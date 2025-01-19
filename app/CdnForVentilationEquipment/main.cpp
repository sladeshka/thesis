#include <iostream>
#include <fstream>
#include <string>
#include "httplib.h"
#include "sqlite3.h"

void createTable(sqlite3* db) {
    const char* sql = R"(CREATE TABLE IF NOT EXISTS files (
            id TEXT PRIMARY KEY,
            extension TEXT NOT NULL,
            path TEXT NOT NULL
        );)";

    char* errMsg;
    if (sqlite3_exec(db, sql, 0, 0, &errMsg) != SQLITE_OK) {
        std::cerr << "SQL error: " << errMsg << std::endl;
        sqlite3_free(errMsg);
    }
}

void saveFileToDB(sqlite3* db, const std::string& id, const std::string& extension, const std::string& path) {
    std::string sql = "INSERT INTO files (id, extension, path) VALUES (?, ?, ?);";
    sqlite3_stmt* stmt;

    sqlite3_prepare_v2(db, sql.c_str(), -1, &stmt, nullptr);
    sqlite3_bind_text(stmt, 1, id.c_str(), -1, SQLITE_STATIC);
    sqlite3_bind_text(stmt, 2, extension.c_str(), -1, SQLITE_STATIC);
    sqlite3_bind_text(stmt, 3, path.c_str(), -1, SQLITE_STATIC);

    sqlite3_step(stmt);
    sqlite3_finalize(stmt);
}

int main() {
    sqlite3* db;
    if (sqlite3_open("files.db", &db)) {
        std::cerr << "Can't open database: " << sqlite3_errmsg(db) << std::endl;
        return 1;
    }

    createTable(db);

    httplib::Server server;

    server.Get(R"(/)", [&](const httplib::Request& req, httplib::Response& res) {
        res.status = 200;
        res.set_content("Ok", "text/plain");
    });

    server.Get(R"(/api/v1/files/([\w-]+)/src\.([\w]+))", [&](const httplib::Request& req, httplib::Response& res) {
        std::string id = req.matches[1];
        std::string path;
        std::string sql = "SELECT path FROM files WHERE id = ?;";
        sqlite3_stmt* stmt;

        sqlite3_prepare_v2(db, sql.c_str(), -1, &stmt, nullptr);
        sqlite3_bind_text(stmt, 1, id.c_str(), -1, SQLITE_STATIC);

        if (sqlite3_step(stmt) == SQLITE_ROW) {
            path = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0));
        }
        sqlite3_finalize(stmt);

        std::ifstream file(path, std::ios::binary);
        if (file) {
            res.set_file_content(path, "image/jpg");
        }
        else {
            res.status = 404;
            res.set_content("File not found", "text/plain");
        }
    });

    server.Post(R"(/api/v1/files/([\w-]+)/src\.([\w]+))", [&](const httplib::Request& req, httplib::Response& res) {
        std::string id = req.matches[1];
        std::string extension = req.matches[2];
        std::string path = "./uploads/" + id + "." + extension;

        std::ofstream ofs(path, std::ios::binary);
        ofs << req.body;
        ofs.close();

        saveFileToDB(db, id, extension, path);
        res.status = 200;
        res.set_content("{\"code\": \"200\", \"message\" : \"ok\"}", "application/json");
    });

    server.listen("localhost", 8080);

    sqlite3_close(db);
    return 0;
}