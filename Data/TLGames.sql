CREATE SCHEMA IF NOT EXISTS TLGames
DEFAULT CHARACTER SET utf8mb4
DEFAULT COLLATE utf8mb4_general_ci;
-- DROP DATABASE IF EXISTS TLGames;
USE TLGames;

-- ======================================================== USERS ========================================================
-- Roles
CREATE TABLE IF NOT EXISTS `roles` (
     `role_id` INT UNSIGNED AUTO_INCREMENT,
     `role_name` VARCHAR(255) NOT NULL,
     PRIMARY KEY (`role_id`)
) ENGINE = InnoDB;

-- users
CREATE TABLE IF NOT EXISTS `users` (
     `user_id` INT UNSIGNED AUTO_INCREMENT,
     `user_name` VARCHAR(50) NOT NULL,
     `password` VARCHAR (50) NOT NULL,
     `create_date` DATETIME NOT NULL,
     PRIMARY KEY (`user_id`)
) ENGINE = InnoDB;

-- user's storage
CREATE TABLE IF NOT EXISTS `user_storages` (
     `user_storage_id` INT UNSIGNED AUTO_INCREMENT,
     `user_id` INT UNSIGNED,
     PRIMARY KEY(`user_storage_id`)
) ENGINE = InnoDB;

-- user's wallet
CREATE TABLE IF NOT EXISTS `wallets` (
     `wallet_id` INT UNSIGNED AUTO_INCREMENT,
     `user_id` INT UNSIGNED NOT NULL,
     `balance` DECIMAL(15, 3),
     `currency` VARCHAR(50) NOT NULL,
     `last_update_balance_date` DATETIME DEFAULT NULL,
     PRIMARY KEY(`wallet_id`),
     FOREIGN KEY(`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- user's cart 
CREATE TABLE IF NOT EXISTS `carts` (
     `cart_id` INT UNSIGNED AUTO_INCREMENT,
     `user_id` INT UNSIGNED NOT NULL,
     `total_price` DECIMAL(15, 3) DEFAULT 0,
     PRIMARY KEY(`cart_id`),
     FOREIGN KEY(`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- user's relationships
CREATE TABLE IF NOT EXISTS `user_relationships` (
     `requester_id` INT UNSIGNED NOT NULL,
     `receiver_id` INT UNSIGNED NOT NULL,
     `request_date` DATETIME NOT NULL,
     `accept_date` DATETIME NOT NULL,
     `status` ENUM('pending', 'accepted', 'rejected') DEFAULT 'pending',
     PRIMARY KEY(`requester_id`, `receiver_id`),
     FOREIGN KEY (`requester_id`) REFERENCES users(`user_id`),
     FOREIGN KEY (`receiver_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- middle table users - roles
CREATE TABLE IF NOT EXISTS `role_of_users` (
     `user_id` INT UNSIGNED NOT NULL,
     `role_id` INT UNSIGNED NOT NULL,
     `create_date` DATETIME NOT NULL,
     PRIMARY KEY (`user_id`, `role_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`),
     FOREIGN KEY (`role_id`) REFERENCES roles(`role_id`)
) ENGINE = InnoDB;

-- customers
CREATE TABLE IF NOT EXISTS `customers` (
     `customer_id` VARCHAR(255) NOT NULL,
     `user_id` INT UNSIGNED NOT NULL,
     `age` INT UNSIGNED DEFAULT NULL,
     `personal_phone` VARCHAR(15) DEFAULT NULL,
     `personal_name` VARCHAR(100) DEFAULT NULL,
     `personal_address` VARCHAR(255) DEFAULT NULL,
     `avatar_url` VARCHAR(255) DEFAULT NULL,
     `background_url` VARCHAR(255) DEFAULT NULL,
     `biology` BOOLEAN DEFAULT NULL,
     PRIMARY KEY(`customer_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- developers
CREATE TABLE IF NOT EXISTS `developers` (
     `developer_id` VARCHAR(255) NOT NULL,
     `user_id` INT UNSIGNED NOT NULL,
     `developer_name` VARCHAR(150) DEFAULT NULL,
     `became_developer_date` DATETIME NOT NULL,
     `description` TEXT DEFAULT NULL,
     `website_url` VARCHAR(255) DEFAULT NULL,
     `studio_phone` VARCHAR(15) DEFAULT NULL,
     `studio_address` VARCHAR(255) DEFAULT NULL,
     `studio_email` VARCHAR(100) DEFAULT NULL,
     PRIMARY KEY(`developer_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- middle table users - developers
CREATE TABLE IF NOT EXISTS `follower_of_developers` (
     `follower_id` INT UNSIGNED NOT NULL,
     `developer_id` VARCHAR(255) NOT NULL,
     `start_follow_date` DATETIME NOT NULL,
     PRIMARY KEY (`follower_id`, `developer_id`),
     FOREIGN KEY (`follower_id`) REFERENCES users(`user_id`),
     FOREIGN KEY (`developer_id`) REFERENCES developers(`developer_id`)
) ENGINE = InnoDB;

-- publishers
CREATE TABLE IF NOT EXISTS `publishers` (
     `publisher_id` VARCHAR(255) NOT NULL,
     `user_id` INT UNSIGNED NOT NULL,
     `publisher_name` VARCHAR(150) DEFAULT NULL,
     `became_publisher_date` DATETIME NOT NULL,
     `description` TEXT DEFAULT NULL,
     `website_url` VARCHAR(255) DEFAULT NULL,
     `business_phone` VARCHAR(15) DEFAULT NULL,
     `business_address` VARCHAR(255) DEFAULT NULL,
     `business_email` VARCHAR(100) DEFAULT NULL,
     PRIMARY KEY(`publisher_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- middle table users - publishers
CREATE TABLE IF NOT EXISTS `follower_of_publishers` (
     `follower_id` INT UNSIGNED NOT NULL,
     `publisher_id` VARCHAR(255) NOT NULL,
     `start_follow_date` DATETIME NOT NULL,
     PRIMARY KEY (`follower_id`, `publisher_id`),
     FOREIGN KEY (`follower_id`) REFERENCES users(`user_id`),
     FOREIGN KEY (`publisher_id`) REFERENCES publishers(`publisher_id`)
) ENGINE = InnoDB;

-- ======================================================== PRODUCTS ========================================================
-- products
CREATE TABLE IF NOT EXISTS `products` (
     `product_id` INT UNSIGNED NOT NULL,
     `developer_id` VARCHAR(255) NOT NULL,
     `product_name` VARCHAR(255) NOT NULL,
     `release_date` DATETIME NOT NULL,
     `base_price` DECIMAL(8,3) NOT NULL,
     `trailer_url` VARCHAR(255) DEFAULT NULL,
     `game_mode` ENUM('single_player', 'multi_player') DEFAULT 'single_player',
     `rating_age` INT UNSIGNED DEFAULT NULL, 
     `status` BOOLEAN DEFAULT 1,
     PRIMARY KEY(`product_id`),
     FOREIGN KEY(`developer_id`) REFERENCES developers(`developer_id`)
)ENGINE = InnoDB;

-- product's images
CREATE TABLE IF NOT EXISTS `product_images` (
     `product_id` INT UNSIGNED NOT NULL,
     `image_url` VARCHAR(255) NOT NULL,
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB;

-- product's version
CREATE TABLE IF NOT EXISTS `product_versions`(
     `product_version_id` INT UNSIGNED AUTO_INCREMENT ,
     `product_id` INT UNSIGNED NOT NULL,
     `executable_path` VARCHAR(50) NOT NULL,
     `download_url` TEXT NOT NULL, -- VARCHAR OR TEXT 
     `upload_date` DATETIME DEFAULT CURRENT_TIMESTAMP, 
     `version` VARCHAR(100) NOT NULL,
     `launch_args` VARCHAR (255) NOT NULL,
     `size_mb` INT UNSIGNED NOT NULL,
     `update_description` TEXT DEFAULT NULL,
     PRIMARY KEY(`product_version_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB;

-- categories
CREATE TABLE IF NOT EXISTS `categories` (
     `category_id` INT UNSIGNED AUTO_INCREMENT,
     `category_name` VARCHAR(80) NOT NULL,
     PRIMARY KEY(`category_id`)
) ENGINE = InnoDB; 

-- product's categories
CREATE TABLE IF NOT EXISTS `product_categories` (
     `product_id` INT UNSIGNED NOT NULL,
     `category_id` INT UNSIGNED NOT NULL,
     PRIMARY KEY(`product_id`, `category_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`),
     FOREIGN KEY(`category_id`) REFERENCES categories(`category_id`)
) ENGINE = InnoDB;

-- System requirement
CREATE TABLE IF NOT EXISTS `system_requirements` (
     `system_requirement_id` INT UNSIGNED NOT NULL,
     `product_id` INT UNSIGNED NOT NULL,
     `minimum_os` NVARCHAR(60) DEFAULT NULL,
     `recommend_os` NVARCHAR(60) DEFAULT NULL,
     `minimum_processor` NVARCHAR(60) DEFAULT NULL,
     `recommend_processor` NVARCHAR(60) DEFAULT NULL,
     `minimum_memory` NVARCHAR(60) DEFAULT NULL,
     `recommend_memory` NVARCHAR(60) DEFAULT NULL,
     `minimum_graphics` NVARCHAR(60) DEFAULT NULL,
     `recommend_graphics` NVARCHAR(60) DEFAULT NULL,
     `minimum_directX` NVARCHAR(60) DEFAULT NULL,
     `recommend_directX` NVARCHAR(60) DEFAULT NULL,
     `minimum_storage` NVARCHAR(60) DEFAULT NULL,
     `recommend_storage` NVARCHAR(60) DEFAULT NULL,
     PRIMARY KEY(`system_requirement_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB;

-- product's description
CREATE TABLE IF NOT EXISTS `descriptions` (
    `description_id` INT UNSIGNED AUTO_INCREMENT,
    `product_id` INT UNSIGNED NOT NULL,
    `title` TEXT DEFAULT NULL,
    `content` TEXT DEFAULT NULL,
    `last_update_date` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`description_id`),
    FOREIGN KEY (`product_id`) REFERENCES products(`product_id`)
) ENGINE=InnoDB;

-- description's images
CREATE TABLE IF NOT EXISTS `description_images` (
    `description_id` INT UNSIGNED NOT NULL,
    `image_url` VARCHAR(255),
    FOREIGN KEY (`description_id`) REFERENCES descriptions(`description_id`)
) ENGINE=InnoDB;

-- product's social media
CREATE TABLE IF NOT EXISTS `social_media_of_products` (
     `product_id` INT UNSIGNED NOT NULL,
     `social_media_name` VARCHAR(50) NOT NULL,
     `social_media_url` VARCHAR(255) NOT NULL,
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB;

-- middle table products - publishers
CREATE TABLE IF NOT EXISTS `publisher_of_products` (
     `product_id` INT UNSIGNED NOT NULL,
     `publisher_id` VARCHAR(255) NOT NULL,
     PRIMARY KEY(`product_id`, `publisher_id`),
     FOREIGN KEY (`product_id`) REFERENCES products(`product_id`),
     FOREIGN KEY (`publisher_id`) REFERENCES publishers(`publisher_id`)
) ENGINE = InnoDB;

-- middle table products - developers
CREATE TABLE IF NOT EXISTS `developer_of_products` (
     `product_id` INT UNSIGNED NOT NULL,
     `developer_id` VARCHAR(255) NOT NULL,
     PRIMARY KEY(`product_id`, `developer_id`),
     FOREIGN KEY (`product_id`) REFERENCES products(`product_id`),
     FOREIGN KEY (`developer_id`) REFERENCES developers(`developer_id`)
) ENGINE = InnoDB;

-- middle table products - categories
CREATE TABLE IF NOT EXISTS `category_of_products` (
     `product_id` INT UNSIGNED NOT NULL,
     `category_id` INT UNSIGNED NOT NULL,
     PRIMARY KEY(`product_id`, `category_id`),
     FOREIGN KEY (`product_id`) REFERENCES products(`product_id`),
     FOREIGN KEY (`category_id`) REFERENCES categories(`category_id`)
) ENGINE = InnoDB;

-- middle table products - storages
CREATE TABLE IF NOT EXISTS `detail_user_storages` (
     `product_id` INT UNSIGNED NOT NULL,
     `user_storage_id` INT UNSIGNED NOT NULL,
     `last_played` DATETIME DEFAULT NULL,
     `play_time` DATETIME DEFAULT NULL,
     `is_favored` BOOLEAN DEFAULT 0,
     `purchase_date` DATETIME NOT NULL,
     `is_installed` BOOLEAN DEFAULT 0,
     `installed_date` DATETIME DEFAULT NULL,
     PRIMARY KEY(`product_id`, `user_storage_id`),
     FOREIGN KEY(`user_storage_id`) REFERENCES user_storages(`user_storage_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB; 

-- middle table products - carts
CREATE TABLE IF NOT EXISTS `detail_carts` (
     `cart_id` INT UNSIGNED NOT NULL,
     `product_id` INT UNSIGNED NOT NULL,
     `add_date` DATETIME NOT NULL,
     `price` DECIMAL(8, 3) NOT NULL,
     `type` ENUM('buy', 'gift') DEFAULT 'buy',
     PRIMARY KEY(`product_id`, `cart_id`),
     FOREIGN KEY(`cart_id`) REFERENCES carts(`cart_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB; 

-- ======================================================== NEWS ========================================================
-- news categories
CREATE TABLE IF NOT EXISTS `news_categories` (
    `news_category_id` INT UNSIGNED AUTO_INCREMENT,
    `category_name` VARCHAR(50),
    PRIMARY KEY (`news_category_id`)
) ENGINE=InnoDB;

-- news
CREATE TABLE IF NOT EXISTS `news` (
    `news_id` INT UNSIGNED AUTO_INCREMENT,
    `user_id` INT UNSIGNED NOT NULL,
    `news_category_id` INT UNSIGNED NOT NULL,
    `related_product_id` INT UNSIGNED DEFAULT NULL,
    `title` VARCHAR(255),
    `published_date` DATETIME NOT NULL,
    `content` TEXT,
    `status` BOOLEAN DEFAULT 1,
    PRIMARY KEY (`news_id`),
    FOREIGN KEY (`user_id`) REFERENCES users(`user_id`),
    FOREIGN KEY (`news_category_id`) REFERENCES news_categories(`news_category_id`),
    FOREIGN KEY (`related_product_id`) REFERENCES products(`product_id`)
) ENGINE=InnoDB;

-- news images
CREATE TABLE IF NOT EXISTS `news_images` (
    `news_id` INT UNSIGNED NOT NULL,
    `image_url` VARCHAR(255) NOT NULL,
    FOREIGN KEY (`news_id`) REFERENCES news(`news_id`)
) ENGINE=InnoDB;

-- ======================================================== PAYMENTS ========================================================
-- banks
CREATE TABLE IF NOT EXISTS `banks` (
     `bank_id` INT UNSIGNED AUTO_INCREMENT,
     `bank_name` VARCHAR(50) NOT NULL,
     `status` BOOLEAN DEFAULT 1,
     PRIMARY KEY(`bank_id`)
) ENGINE = InnoDB;

-- payment methods
CREATE TABLE IF NOT EXISTS `payment_methods` (
    `payment_method_id` INT UNSIGNED AUTO_INCREMENT,
    `payment_method_type` ENUM('Visa Or MasterCard', 'banking', 'momo') DEFAULT NULL,
    `bank_id` INT UNSIGNED NOT NULL,
    `display_name` VARCHAR(50),
    `last_four_digit` VARCHAR(4),
    `expiry_year` INT,
    `expiry_month` INT,
    `token` VARCHAR(50),
    PRIMARY KEY (`payment_method_id`),
    FOREIGN KEY(`bank_id`) REFERENCES banks(`bank_id`)
) ENGINE=InnoDB;

-- middle table users - payment methods
CREATE TABLE IF NOT EXISTS payment_of_users (
    `user_id` INT UNSIGNED NOT NULL,
    `payment_method_id` INT UNSIGNED NOT NULL,
    `added_date` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `last_updated_date` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `status` BOOLEAN DEFAULT 1,
     PRIMARY KEY(`user_id`, `payment_method_id`),
    FOREIGN KEY (`user_id`) REFERENCES users(`user_id`),
    FOREIGN KEY (`payment_method_id`) REFERENCES payment_methods(`payment_method_id`)
) ENGINE=InnoDB;

-- ======================================================== SALE EVENTS ========================================================
-- sale events
CREATE TABLE IF NOT EXISTS `sale_events` (
    `sale_event_id` INT UNSIGNED AUTO_INCREMENT,
    `discount_code` VARCHAR(255) NOT NULL UNIQUE,
    `start_date` DATETIME,
    `end_date` DATETIME,
    `sale_event_name` VARCHAR(100),
    `status` BOOLEAN DEFAULT 0,
    `description` TEXT,
    PRIMARY KEY (sale_event_id)
) ENGINE=InnoDB;

-- sale event details
CREATE TABLE IF NOT EXISTS `detail_sale_events` (
    `sale_event_id` INT UNSIGNED AUTO_INCREMENT,
    `product_id` INT UNSIGNED NOT NULL,
    `discount_type` ENUM('percent', 'amount') NOT NULL,
    `discount_percent` DECIMAL(3,3),
    `discount_amount` DECIMAL(8,3),
    `max_discount_price` DECIMAL(8,3),
    `min_price_to_use` DECIMAL(8,3),
     PRIMARY KEY(`product_id`, `sale_event_id`),
    FOREIGN KEY (`sale_event_id`) REFERENCES sale_events(`sale_event_id`),
    FOREIGN KEY (`product_id`) REFERENCES products(`product_id`),
    CONSTRAINT check_discount_type CHECK
    (
          (`discount_type` = 'percent' AND `discount_percent` IS NOT NULL AND `discount_amount` IS NULL) 
     OR
          (`discount_type` = 'amount' AND `discount_percent` IS NULL AND `discount_amount` IS NOT NULL)
    )
) ENGINE=InnoDB;

-- platform rules
CREATE TABLE IF NOT EXISTS `platform_rules` (
     `platform_rule_id` INT UNSIGNED AUTO_INCREMENT,
     `fee` DECIMAL(3, 3) NOT NULL DEFAULT 0,
     `pending_time` INT NOT NULL DEFAULT 0,
     PRIMARY KEY(`platform_rule_id`)
) ENGINE = InnoDB;

-- ======================================================== INVOICES ========================================================
-- invoices
CREATE TABLE IF NOT EXISTS `invoices` (
    `invoice_id` INT UNSIGNED AUTO_INCREMENT,
    `customer_id` VARCHAR(255) NOT NULL,
    `payment_method_id` INT UNSIGNED NOT NULL,
    `total_price` DECIMAL(12, 3) NOT NULL,
    `invoice_date` DATETIME NOT NULL,
    `status` ENUM('return, success, cancel') NOT NULL,
    PRIMARY KEY(`invoice_id`),
    FOREIGN KEY(`customer_id`) REFERENCES customers(`customer_id`),
    FOREIGN KEY(`payment_method_id`) REFERENCES payment_methods(`payment_method_id`) 
) ENGINE=InnoDB;

-- detail invoices
CREATE TABLE IF NOT EXISTS `detail_invoices` (
    `invoice_id` INT UNSIGNED NOT NULL,
    `discount_code` VARCHAR(255) NOT NULL UNIQUE,
    `product_id` INT UNSIGNED NOT NULL,
    `quantity` INT DEFAULT 1,
    `price` DECIMAL(12, 3) NOT NULL,
    `type` ENUM('return', 'success') NOT NULL DEFAULT 'success',
     PRIMARY KEY(`product_id`, `invoice_id`),
    FOREIGN KEY(`invoice_id`) REFERENCES invoices(`invoice_id`),
    FOREIGN KEY(`discount_code`) REFERENCES sale_events(`discount_code`),
    FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE=InnoDB;

-- transactions
CREATE TABLE IF NOT EXISTS `transactions` (
    `transaction_id` INT UNSIGNED AUTO_INCREMENT,
    `wallet_id` INT UNSIGNED NOT NULL,
    `invoice_id` INT UNSIGNED NOT NULL,
    `platform_rule_id` INT UNSIGNED,
    `transaction_date` DATETIME NOT NULL,
    `transaction_type` ENUM('return','purchase', 'sell') NOT NULL DEFAULT 'purchase',
    `current_balance` DECIMAL(12, 3) NOT NULL,
    `status` ENUM('return', 'success', 'pending') NOT NULL DEFAULT 'pending',
    PRIMARY KEY(`transaction_id`),
    FOREIGN KEY(`invoice_id`) REFERENCES invoices(`invoice_id`),
    FOREIGN KEY(`wallet_id`) REFERENCES wallets(`wallet_id`),
    FOREIGN KEY(`platform_rule_id`) REFERENCES platform_rules(`platform_rule_id`)
) ENGINE=InnoDB;

-- ======================================================== REVIEWS ========================================================
-- reviews
CREATE TABLE IF NOT EXISTS `reviews` (
     `review_id` INT UNSIGNED AUTO_INCREMENT,
     `product_id` INT UNSIGNED NOT NULL,
     `user_id` INT UNSIGNED NOT NULL,
     `content` TEXT,
     `upload_date` DATETIME DEFAULT CURRENT_TIMESTAMP,
     `last_update_date` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     `content_rate` DECIMAL(2, 1) COMMENT 'rating this review from 0.5 to 5.0, in 0.5 increments',
     `review_rate` DECIMAL(2, 1) COMMENT 'rating this product from 0.5 to 5.0, in 0.5 increments',
     PRIMARY KEY(`review_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE=InnoDB;

-- reply reviews
CREATE TABLE IF NOT EXISTS `reply_reviews` (
     `reply_id` INT UNSIGNED AUTO_INCREMENT,
     `review_id` INT UNSIGNED NOT NULL,
     `user_id` INT UNSIGNED NOT NULL,
     `content` TEXT,
     `upload_date` DATETIME DEFAULT CURRENT_TIMESTAMP,
     `last_update_date` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     `content_rate` DECIMAL(2, 1) COMMENT 'rating this review from 0.5 to 5.0, in 0.5 increments',
     PRIMARY KEY(`reply_id`),
     FOREIGN KEY(`review_id`) REFERENCES reviews(`review_id`),
     FOREIGN KEY(`user_id`) REFERENCES users(`user_id`)
) ENGINE=InnoDB;

-- ======================================================== CONVERSATIONS ========================================================
-- conversations
CREATE TABLE IF NOT EXISTS `conversations` (
    `conversation_id` INT UNSIGNED AUTO_INCREMENT,
    `first_user_id` INT UNSIGNED NOT NULL,
    `second_user_id` INT UNSIGNED NOT NULL,
    `start_time` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `last_message_time` DATETIME,
    status ENUM('active', 'archived') DEFAULT 'active',
	PRIMARY KEY(`conversation_id`),
    FOREIGN KEY (`first_user_id`) REFERENCES users(user_id),
    FOREIGN KEY (`second_user_id`) REFERENCES users(user_id)
) ENGINE=InnoDB;

-- conversation's participants
CREATE TABLE IF NOT EXISTS `conversation_participants` (
    `conversation_id` INT UNSIGNED NOT NULL,
    `user_id` INT UNSIGNED NOT NULL,
    `last_read_time` DATETIME,
    `join_time` DATETIME DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(`conversation_id`, `user_id`),
    FOREIGN KEY (`conversation_id`) REFERENCES conversations(`conversation_id`) ON DELETE CASCADE,
    FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE=InnoDB;

-- conversation's messages
CREATE TABLE IF NOT EXISTS `messages` (
    `message_id` INT UNSIGNED AUTO_INCREMENT,
    `replied_message_id` INT UNSIGNED,
    `send_user_id` INT UNSIGNED NOT NULL,
    `conversation_id` INT UNSIGNED NOT NULL,
    `content` TEXT,
    `send_time` DATETIME DEFAULT CURRENT_TIMESTAMP,
    `message_type` ENUM('text', 'image', 'video', 'file') DEFAULT 'text',
    `attachment_url` VARCHAR(255),
	PRIMARY KEY(`message_id`),
    FOREIGN KEY (`replied_message_id`) REFERENCES messages(`message_id`) ON DELETE SET NULL,
    FOREIGN KEY (`send_user_id`) REFERENCES users(`user_id`),
    FOREIGN KEY (`conversation_id`) REFERENCES conversations(`conversation_id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ======================================================== INDEXES ========================================================
-- banks index
CREATE INDEX `idx_banks_bank_name` ON banks(bank_name);

-- sale_events's index
CREATE INDEX `idx_sale_events_sale_event_name` ON sale_events(sale_event_name);

-- invoices's indexes
CREATE INDEX `idx_invoices_invoice_date` ON invoices(invoice_date);

-- transactions's indexes
CREATE INDEX `idx_transactions_transaction_date` ON transactions(transaction_date);

-- conversation's index
CREATE INDEX `idx_last_message_time` ON conversations(`last_message_time`);

-- ======================================================== TRIGGERS ========================================================
-- conversation's triggers
DELIMITER $$
CREATE TRIGGER trg_update_last_message_time
AFTER INSERT ON messages
FOR EACH ROW
BEGIN
     UPDATE conversations
     SET last_message_time = NEW.send_time
     WHERE conversation_id = NEW.conversation_id;
END$$
DELIMITER ;

DELIMITER $$
CREATE TRIGGER trg_set_join_last_read
BEFORE INSERT ON conversation_participants
FOR EACH ROW
BEGIN
    IF NEW.last_read_time IS NULL THEN
        SET NEW.last_read_time = NOW();
    END IF;

    IF NEW.join_time IS NULL THEN
        SET NEW.join_time = NOW();
    END IF;
END$$
DELIMITER ;

-- ======================================================== INSERT DATA ========================================================
-- roles
INSERT INTO `roles` (`role_name`) VALUES
('customer'),
('developer'),
('publisher'),
('admin');

-- users
INSERT INTO `users` (`user_name`, `password`, `create_date`) VALUES
('alice_123', 'pass123', '2024-01-15 10:30:00'),
('bob_456', 'secure456', '2024-02-20 14:45:00'),
('charlie789', 'complex789', '2024-03-10 09:15:00'),
('david_01', 'simple01', '2024-04-05 16:00:00'),
('eve_xyz', 'evepass', '2024-05-12 11:20:00'),
('franklin_22', 'franklin22', '2024-06-28 18:50:00'),
('grace_g', 'graceful', '2024-07-19 07:40:00'),
('henry_h', 'henry88', '2024-08-03 12:55:00'),
('isabella_i', 'isabella99', '2024-09-25 20:10:00'),
('jack_j', 'jack10', '2024-10-31 13:30:00');

-- user's storages
INSERT INTO `user_storages` (`user_id`) VALUES
(1),
(2),
(3),
(4),
(5),
(6),
(7),
(8),
(9),
(10);

-- wallets
INSERT INTO `wallets` (`user_id`, `balance`, `currency`, `last_update_balance_date`) VALUES
(1, 150.750, 'USD', '2024-01-16 09:00:00'),
(2, 225.500, 'USD', '2024-02-21 15:30:00'),
(3, 50.200, 'EUR', '2024-03-11 10:00:00'),
(4, 500.000, 'USD', '2024-04-06 17:00:00'),
(5, 75.900, 'EUR', '2024-05-13 12:45:00'),
(6, 300.100, 'USD', '2024-06-29 19:15:00'),
(7, 180.300, 'CAD', '2024-07-20 08:00:00'),
(8, 95.650, 'USD', '2024-08-04 14:00:00'),
(9, 420.800, 'CAD', '2024-09-26 21:30:00'),
(10, 110.000, 'USD', '2024-11-01 14:45:00');

-- carts
INSERT INTO `carts` (`user_id`, `total_price`) VALUES
(1, 25.990),
(2, 55.200),
(3, 10.500),
(4, 120.750),
(5, 5.990),
(6, 78.300),
(7, 32.150),
(8, 45.000),
(9, 90.400),
(10, 18.600);

-- user's relationships
INSERT INTO `user_relationships` (`requester_id`, `receiver_id`, `request_date`, `accept_date`, `status`) VALUES
(1, 2, '2024-01-18 11:00:00', '2024-01-19 10:00:00', 'accepted'),
(3, 1, '2024-03-15 15:30:00', '2024-03-16 16:45:00', 'accepted'),
(4, 5, '2024-04-10 09:00:00', '2024-04-12 12:00:00', 'accepted'),
(6, 3, '2024-07-01 20:00:00', '2024-07-03 11:15:00', 'accepted'),
(7, 9, '2024-08-08 14:30:00', '2024-08-10 17:00:00', 'accepted'),
(8, 10, '2024-08-15 18:00:00', '2024-08-17 09:30:00', 'accepted'),
(2, 5, '2024-02-25 12:15:00', '2024-02-28 16:00:00', 'rejected'),
(9, 6, '2024-09-28 10:45:00', '2024-10-01 13:00:00', 'pending'),
(5, 7, '2024-05-15 16:30:00', '2024-05-18 08:00:00', 'accepted'),
(10, 4, '2024-11-05 11:00:00', '2024-11-07 15:45:00', 'accepted');

--  role of user
INSERT INTO `role_of_users` (`user_id`, `role_id`, `create_date`) VALUES
(1, 1, '2024-01-15 10:35:00'),
(2, 1, '2024-02-20 14:50:00'),
(2, 2, '2024-02-20 14:50:00'),
(3, 1, '2024-03-10 09:20:00'),
(3, 3, '2024-03-10 09:20:00'),
(4, 1, '2024-04-05 16:05:00'),
(4, 3, '2024-04-05 16:05:00'),
(5, 1, '2024-05-12 11:25:00'),
(5, 2, '2024-05-12 11:25:00'),
(6, 1, '2024-06-28 18:55:00'),
(6, 3, '2024-06-28 18:55:00'),
(7, 1, '2024-07-19 07:45:00'),
(8, 1, '2024-08-03 13:00:00'),
(8, 2, '2024-08-03 13:00:00'),
(9, 1, '2024-09-25 20:15:00'),
(9, 3, '2024-09-25 20:15:00'),
(10, 4, '2024-10-31 13:35:00');

-- customer
INSERT INTO `customers` (`customer_id`, `user_id`, `age`, `personal_phone`, `personal_name`, `personal_address`, `avatar_url`, `background_url`, `biology`) VALUES
('cust001', 1, 28, '0901234567', 'User One', 'Address One', 'avatar_one.jpg', 'bg_one.jpg', NULL),
('cust002', 2, 35, '0919876543', 'User Two', 'Address Two', 'avatar_two.png', 'bg_two.png', TRUE),
('cust003', 3, 22, '0981122334', 'User Three', 'Address Three', 'avatar_three.jpeg', 'bg_three.jpeg', FALSE),
('cust004', 4, 41, '0934455667', 'User Four', 'Address Four', 'avatar_four.gif', 'bg_four.gif', NULL),
('cust005', 5, 30, '0977788990', 'User Five', 'Address Five', 'avatar_five.bmp', 'bg_five.bmp', TRUE),
('cust006', 6, 26, '0965551212', 'User Six', 'Address Six', 'avatar_six.webp', 'bg_six.webp', FALSE),
('cust007', 7, 39, '0941122333', 'User Seven', 'Address Seven', 'avatar_seven.tiff', 'bg_seven.tiff', NULL),
('cust008', 8, 24, '0928899000', 'User Eight', 'Address Eight', 'avatar_eight.svg', 'bg_eight.svg', TRUE),
('cust009', 9, 33, '0956543210', 'User Nine', 'Address Nine', 'avatar_nine.psd', 'bg_nine.psd', FALSE),
('cust010', 10, 45, '0991020304', 'User Ten', 'Address Ten', 'avatar_ten.ai', 'bg_ten.ai', NULL);