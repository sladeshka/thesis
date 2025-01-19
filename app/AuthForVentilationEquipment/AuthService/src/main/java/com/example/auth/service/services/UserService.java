package com.example.auth.service.services;
import com.example.auth.service.models.User;
import com.example.auth.service.repositories.UserRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.orm.ObjectOptimisticLockingFailureException;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.util.Optional;
import java.util.UUID;

@Service
public class UserService {
    private static final Logger log = LoggerFactory.getLogger(UserService.class);

    @Autowired
    private UserRepository userRepository;

    private final BCryptPasswordEncoder passwordEncoder = new BCryptPasswordEncoder();

    @Transactional(propagation = Propagation.REQUIRES_NEW)
    public User registerUser(User user) {
        try {
            Optional<User> existingUser = userRepository.findByLogin(user.getLogin());
            if (existingUser.isPresent()) {
                return null;
            }
            if (user.getVersion() == null) {
                user.setVersion(0);
            }
            user.setVersion(user.getVersion() + 1);
            user.setPassword(passwordEncoder.encode(user.getPassword()));
            return userRepository.save(user);
        } catch (ObjectOptimisticLockingFailureException e) {
            log.warn("Optimistic locking failure: {}", e.getMessage());
        } catch (Exception e){
            log.warn("An error occurred during user registration: " , e);
        }
        return null;
    }

    public User authenticateUser(String login, String password) {
        User user = userRepository.findByLogin(login).orElse(null);
        if (user != null && passwordEncoder.matches(password, user.getPassword())) {
            return user;
        }
        return null;
    }
}