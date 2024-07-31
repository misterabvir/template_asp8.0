-- Active: 1722319631980@@127.0.0.1@3306

USE `mail_db`;


DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`(
    `user_id` VARCHAR(36) NOT NULL PRIMARY KEY,
    `username` VARCHAR(255) NOT NULL,
    `email` VARCHAR(255) NOT NULL UNIQUE
);

DROP TABLE IF EXISTS `email_templates`;
CREATE TABLE `email_templates`(
    `template_id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `type` VARCHAR(255) NOT NULL,
    `header` VARCHAR(255) NOT NULL,
    `body` TEXT NOT NULL
);


DROP TABLE IF EXISTS `messages`;
CREATE TABLE `messages`(
    `message_id` VARCHAR(36) NOT NULL PRIMARY KEY,
    `recipient_id` VARCHAR(36) NOT NULL,
    `type` VARCHAR(255) NOT NULL,
    `reason` TEXT NOT NULL,
    `created_at` DATETIME NOT NULL,
    FOREIGN KEY (`recipient_id`) REFERENCES `users`(`user_id`)
);