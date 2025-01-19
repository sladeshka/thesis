package com.example.auth.service.controllers;

import com.example.auth.service.models.Response;
import com.example.auth.service.models.User;
import com.example.auth.service.services.UserService;
import com.auth0.jwt.JWT;
import com.auth0.jwt.algorithms.Algorithm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.Date;
import java.util.UUID;

@RestController
@RequestMapping("/api/v1")
public class UserController {
    @Autowired
    private UserService userService;

    private final String SECRET_KEY = "m%QHoG*(z*7FH#km07CieKKWMG3WO1H9";

    @CrossOrigin
    @PostMapping("/signup")
    public Response signup(@RequestBody User user) {
        Response response = new Response();
        response.setCode(200);
        response.setMessage("Ok");
        response.setData(userService.registerUser(user));
        return response;
    }

    @CrossOrigin
    @PostMapping("/signin")
    public Response signin(@RequestBody User user) {
        Response response = new Response();
        User authUser = userService.authenticateUser(user.getLogin(), user.getPassword());
        if(authUser == null) {
            response.setCode(403);
            response.setMessage("Forbidden");
            return response;
        }

        if(!authUser.isActive()) {
            response.setCode(403);
            response.setMessage("Forbidden");
            return response;
        }

        response.setCode(200);
        response.setMessage("Ok");
        response.setData(generateToken(authUser.getId()));
        return response;
    }

    private String generateToken(UUID userId) {
        long now = System.currentTimeMillis();
        return JWT.create()
                .withClaim("guid", userId.toString())
                .withExpiresAt(new Date(now + 86400000))
                .sign(Algorithm.HMAC256(SECRET_KEY));
    }
}