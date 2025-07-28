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
     `status` ENUM('active', 'inactive') NOT NULL DEFAULT 'active',
     PRIMARY KEY (`role_id`)
) ENGINE = InnoDB;

-- users
CREATE TABLE IF NOT EXISTS `users` (
     `user_id` INT UNSIGNED AUTO_INCREMENT,
     `user_name` VARCHAR(50) NOT NULL,
     `password` VARCHAR (50) NOT NULL,
     `create_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `status` ENUM('active', 'lock', 'inactive') NOT NULL DEFAULT 'active',
     PRIMARY KEY (`user_id`)
) ENGINE = InnoDB;

-- user's storage
CREATE TABLE IF NOT EXISTS `user_storages` (
     `user_storage_id` INT UNSIGNED AUTO_INCREMENT,
     `user_id` INT UNSIGNED NOT NULL,
     PRIMARY KEY(`user_storage_id`),
     FOREIGN KEY(`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- user's wallet
CREATE TABLE IF NOT EXISTS `wallets` (
     `wallet_id` INT UNSIGNED AUTO_INCREMENT,
     `user_id` INT UNSIGNED NOT NULL,
     `balance` DECIMAL(15, 3) NOT NULL DEFAULT 0,
     `currency` VARCHAR(50) NOT NULL DEFAULT 'VND',
     `last_update_balance_date` DATETIME DEFAULT NULL,
     PRIMARY KEY(`wallet_id`),
     FOREIGN KEY(`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- user's relationships
CREATE TABLE IF NOT EXISTS `user_relationships` (
     `requester_id` INT UNSIGNED NOT NULL,
     `receiver_id` INT UNSIGNED NOT NULL,
     `request_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `accept_date` DATETIME DEFAULT NULL,
     `status` ENUM('pending', 'accepted', 'rejected') DEFAULT 'pending',
     PRIMARY KEY(`requester_id`, `receiver_id`),
     FOREIGN KEY (`requester_id`) REFERENCES users(`user_id`),
     FOREIGN KEY (`receiver_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- middle table users - roles
CREATE TABLE IF NOT EXISTS `role_of_users` (
     `user_id` INT UNSIGNED NOT NULL,
     `role_id` INT UNSIGNED NOT NULL,
     `create_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     PRIMARY KEY (`user_id`, `role_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`),
     FOREIGN KEY (`role_id`) REFERENCES roles(`role_id`)
) ENGINE = InnoDB;

-- customers
CREATE TABLE IF NOT EXISTS `customers` (
     `customer_id` INT UNSIGNED AUTO_INCREMENT,
     `user_id` INT UNSIGNED NOT NULL,
     `birthday` DATETIME DEFAULT NULL,
     `personal_phone` VARCHAR(15) NOT NULL DEFAULT '' ,
     `personal_name` VARCHAR(100) NOT NULL DEFAULT '',
     `personal_address` VARCHAR(255) NOT NULL DEFAULT '',
     `avatar_url` VARCHAR(255) NOT NULL DEFAULT '',
     `background_url` VARCHAR(255) NOT NULL DEFAULT '',
     `gender` ENUM('male', 'female', 'other') NOT NULL DEFAULT 'male',
     `status` ENUM('active', 'lock', 'inactive') NOT NULL DEFAULT 'active',
     PRIMARY KEY(`customer_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;


-- user's cart 
CREATE TABLE IF NOT EXISTS `carts` (
     `cart_id` INT UNSIGNED AUTO_INCREMENT,
     `customer_id` INT UNSIGNED NOT NULL,
     `total_price` DECIMAL(15, 3) NOT NULL ,
     PRIMARY KEY(`cart_id`),
     FOREIGN KEY(`customer_id`) REFERENCES customers(`customer_id`)
) ENGINE = InnoDB;

-- developers
CREATE TABLE IF NOT EXISTS `developers` (
     `developer_id` INT UNSIGNED AUTO_INCREMENT,
     `user_id` INT UNSIGNED NOT NULL,
     `developer_name` VARCHAR(150) NOT NULL,
     `became_developer_date` DATETIME NOT NULL,
     `description` TEXT NOT NULL DEFAULT '',
     `website_url` VARCHAR(255) NOT NULL DEFAULT '',
     `studio_phone` VARCHAR(15) NOT NULL DEFAULT '',
     `studio_address` VARCHAR(255) NOT NULL DEFAULT '',
     `studio_email` VARCHAR(100) NOT NULL DEFAULT '',
     `status` ENUM('active', 'lock', 'inactive') DEFAULT 'inactive',
     PRIMARY KEY(`developer_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- middle table users - developers
CREATE TABLE IF NOT EXISTS `follower_of_developers` (
     `follower_id` INT UNSIGNED NOT NULL,
     `developer_id` INT UNSIGNED NOT NULL,
     `start_follow_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     PRIMARY KEY (`follower_id`, `developer_id`),
     FOREIGN KEY (`follower_id`) REFERENCES users(`user_id`),
     FOREIGN KEY (`developer_id`) REFERENCES developers(`developer_id`)
) ENGINE = InnoDB;

-- publishers
CREATE TABLE IF NOT EXISTS `publishers` (
     `publisher_id` INT UNSIGNED AUTO_INCREMENT,
     `user_id` INT UNSIGNED NOT NULL,
     `publisher_name` VARCHAR(150) NOT NULL DEFAULT '',
     `became_publisher_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `description` TEXT NOT NULL DEFAULT '',
     `website_url` TEXT NOT NULL DEFAULT '',
     `business_phone` VARCHAR(15) NOT NULL DEFAULT '',
     `business_address` VARCHAR(255) NOT NULL DEFAULT '',
     `business_email` VARCHAR(100) NOT NULL DEFAULT '',
     `status` ENUM('active', 'lock', 'inactive') DEFAULT 'inactive',
     PRIMARY KEY(`publisher_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE = InnoDB;

-- middle table users - publishers
CREATE TABLE IF NOT EXISTS `follower_of_publishers` (
     `follower_id` INT UNSIGNED NOT NULL,
     `publisher_id` INT UNSIGNED NOT NULL,
     `start_follow_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     PRIMARY KEY (`follower_id`, `publisher_id`),
     FOREIGN KEY (`follower_id`) REFERENCES users(`user_id`),
     FOREIGN KEY (`publisher_id`) REFERENCES publishers(`publisher_id`)
) ENGINE = InnoDB;

-- ======================================================== PRODUCTS ========================================================
-- products
CREATE TABLE IF NOT EXISTS `products` (
     `product_id` INT UNSIGNED NOT NULL,
     `developer_id` INT UNSIGNED NOT NULL,
     `product_name` VARCHAR(255) NOT NULL DEFAULT '',
     `release_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `base_price` DECIMAL(8,3) NOT NULL,
     `trailer_url` TEXT NOT NULL DEFAULT '',
     `game_mode` ENUM('single_player', 'multi_player') NOT NULL DEFAULT 'single_player',
     `rating_age` INT UNSIGNED NOT NULL DEFAULT 0, 
     `status` ENUM('active', 'inactive') NOT NULL DEFAULT 'active',
     `downloaded_count` INT UNSIGNED NOT NULL DEFAULT 0,
     PRIMARY KEY(`product_id`),
     FOREIGN KEY(`developer_id`) REFERENCES developers(`developer_id`)
)ENGINE = InnoDB;

-- product's images
CREATE TABLE IF NOT EXISTS `product_images` (
     `product_image_id` INT UNSIGNED AUTO_INCREMENT,
     `product_id` INT UNSIGNED NOT NULL,
     `image_url` VARCHAR(255) NOT NULL DEFAULT '',
     PRIMARY KEY (`product_image_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB;

-- product's version
CREATE TABLE IF NOT EXISTS `product_versions`(
     `product_version_id` INT UNSIGNED AUTO_INCREMENT ,
     `product_id` INT UNSIGNED NOT NULL,
     `executable_path` VARCHAR(50) NOT NULL DEFAULT '',
     `download_url` TEXT NOT NULL, -- VARCHAR OR TEXT 
     `upload_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, 
     `version` VARCHAR(100) NOT NULL DEFAULT '',
     `launch_args` VARCHAR (255) NOT NULL DEFAULT '',
     `size_mb` FLOAT UNSIGNED NOT NULL DEFAULT 0.0,
     `update_description` TEXT NOT NULL DEFAULT '',
     PRIMARY KEY(`product_version_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB;

-- categories
CREATE TABLE IF NOT EXISTS `categories` (
     `category_id` INT UNSIGNED AUTO_INCREMENT,
     `category_name` VARCHAR(80) NOT NULL,
     `status` ENUM('active', 'inactive') DEFAULT 'active',
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
     `minimum_os` VARCHAR(255) NOT NULL DEFAULT '',
     `recommend_os` VARCHAR(255) NOT NULL DEFAULT '',
     `minimum_processor` VARCHAR(255) NOT NULL DEFAULT '',
     `recommend_processor` VARCHAR(255) NOT NULL DEFAULT '',
     `minimum_memory` VARCHAR(255) NOT NULL DEFAULT '',
     `recommend_memory` VARCHAR(255) NOT NULL DEFAULT '',
     `minimum_graphics` VARCHAR(255) NOT NULL DEFAULT '',
     `recommend_graphics` VARCHAR(255) NOT NULL DEFAULT '',
     `minimum_directX` VARCHAR(255) NOT NULL DEFAULT '',
     `recommend_directX` VARCHAR(255) NOT NULL DEFAULT '',
     `minimum_storage` VARCHAR(255) NOT NULL DEFAULT '',
     `recommend_storage` VARCHAR(255) NOT NULL DEFAULT '',
     PRIMARY KEY(`system_requirement_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB;

-- product's description
CREATE TABLE IF NOT EXISTS `descriptions` (
    `description_id` INT UNSIGNED AUTO_INCREMENT,
    `product_id` INT UNSIGNED NOT NULL,
    `title` TEXT NOT NULL DEFAULT '',
    `content` TEXT NOT NULL DEFAULT '',
    `last_update_date` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`description_id`),
    FOREIGN KEY (`product_id`) REFERENCES products(`product_id`)
) ENGINE=InnoDB;

-- description's images
CREATE TABLE IF NOT EXISTS `description_images` (
     `description_image_id` INT UNSIGNED AUTO_INCREMENT,
    `description_id` INT UNSIGNED NOT NULL,
    `image_url` VARCHAR(255) NOT NULL DEFAULT '',
    PRIMARY KEY (`description_image_id`),
    FOREIGN KEY (`description_id`) REFERENCES descriptions(`description_id`)
) ENGINE = InnoDB;

-- product's social media
CREATE TABLE IF NOT EXISTS `social_media_of_products` (
     `social_media_id` INT UNSIGNED AUTO_INCREMENT,
     `product_id` INT UNSIGNED NOT NULL,
     `account_name` NVARCHAR(255) NOT NULL DEFAULT '', 
     `social_media_type` ENUM('facebook', 'instagram', 'tiktok', 'youtube', 'X', 'discord', 'telegram', 'wechat') DEFAULT NULL,
     `social_media_url` TEXT NOT NULL DEFAULT '',
     PRIMARY KEY (`social_media_id`), 
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB;

-- middle table products - publishers
CREATE TABLE IF NOT EXISTS `publisher_of_products` (
     `product_id` INT UNSIGNED NOT NULL,
     `publisher_id` INT UNSIGNED NOT NULL,
     FOREIGN KEY (`product_id`) REFERENCES products(`product_id`),
     FOREIGN KEY (`publisher_id`) REFERENCES publishers(`publisher_id`)
) ENGINE = InnoDB;

-- middle table products - developers
CREATE TABLE IF NOT EXISTS `developer_of_products` (
     `product_id` INT UNSIGNED NOT NULL,
     `developer_id` INT UNSIGNED NOT NULL,
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
     `is_favored` BOOLEAN NOT NULL DEFAULT 1,
     `purchase_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `is_installed` BOOLEAN NOT NULL DEFAULT 1,
     `installed_date` DATETIME DEFAULT NULL,
     `status` ENUM('active', 'inactive') DEFAULT 'active' NOT NULL,
     PRIMARY KEY(`product_id`, `user_storage_id`),
     FOREIGN KEY(`user_storage_id`) REFERENCES user_storages(`user_storage_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB; 

-- ALTER TABLE `detail_user_storages`
-- ADD COLUMN `status` ENUM('active', 'inactive') NOT NULL DEFAULT 'active';

-- middle table products - carts
CREATE TABLE IF NOT EXISTS `detail_carts` (
     `cart_id` INT UNSIGNED NOT NULL,
     `product_id` INT UNSIGNED NOT NULL,
     `add_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `price` DECIMAL(8, 3) NOT NULL,
     `type` ENUM('buy', 'gift') NOT NULL DEFAULT 'buy',
     PRIMARY KEY(`product_id`, `cart_id`),
     FOREIGN KEY(`cart_id`) REFERENCES carts(`cart_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE = InnoDB; 

-- ======================================================== NEWS ========================================================
-- news categories
CREATE TABLE IF NOT EXISTS `news_categories` (
     `news_category_id` INT UNSIGNED AUTO_INCREMENT,
     `category_name` VARCHAR(50) NOT NULL DEFAULT '',
     `status` ENUM('active', 'inactive') NOT NULL DEFAULT 'active',
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
     `news_image_id` INT UNSIGNED AUTO_INCREMENT,
     `news_id` INT UNSIGNED NOT NULL,
     `image_url` VARCHAR(255) NOT NULL,
     PRIMARY KEY (`news_image_id`),
     FOREIGN KEY (`news_id`) REFERENCES news(`news_id`)
) ENGINE=InnoDB;

-- ======================================================== PAYMENTS ========================================================
-- banks
CREATE TABLE IF NOT EXISTS `banks` (
     `bank_id` INT UNSIGNED AUTO_INCREMENT,
     `bank_name` VARCHAR(255) NOT NULL DEFAULT '',
     `status` ENUM('active', 'inactive') DEFAULT 'active',
     PRIMARY KEY(`bank_id`)
) ENGINE = InnoDB;

-- payment methods
CREATE TABLE IF NOT EXISTS `user_payment_methods` (
    `user_payment_method_id` INT UNSIGNED AUTO_INCREMENT,
    `payment_method_type` ENUM('visa_or_mastercard', 'banking', 'momo') NOT NULL,
    `bank_id` INT UNSIGNED DEFAULT NULL,
     `user_id` INT UNSIGNED NOT NULL,
     `added_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `last_updated_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     `status` ENUM('active', 'inactive') NOT NULL DEFAULT 'active',
     `display_name` VARCHAR(50) NOT NULL DEFAULT '',
     `last_four_digit` VARCHAR(4) NOT NULL DEFAULT '',
     `expiry_year` INT NOT NULL DEFAULT 0,
     `expiry_month` INT NOT NULL DEFAULT 0,
     `token` VARCHAR(255) NOT NULL DEFAULT '',
     CONSTRAINT chk_payment_method_data
     CHECK (
          (`payment_method_type` = 'banking' AND `bank_id` IS NOT NULL AND last_four_digit = '' AND expiry_year = 0 AND expiry_month = 0 AND token = '') OR
          (`payment_method_type` = 'visa_or_mastercard' AND last_four_digit != '' AND expiry_year != 0 AND expiry_month != 0 AND token != '') OR
          (`payment_method_type` = 'momo' AND `bank_id` IS NULL AND last_four_digit = '' AND expiry_year = 0 AND expiry_month = 0 AND token = '')
     ),
     PRIMARY KEY (`user_payment_method_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`),
     FOREIGN KEY(`bank_id`) REFERENCES banks(`bank_id`)
) ENGINE=InnoDB;

-- ======================================================== SALE EVENTS ========================================================
-- sale events
CREATE TABLE IF NOT EXISTS `sale_events` (
     `sale_event_id` INT UNSIGNED AUTO_INCREMENT,
     `discount_code` VARCHAR(255) NOT NULL UNIQUE,
     `start_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `end_date` DATETIME DEFAULT NULL,
     `sale_event_name` VARCHAR(100) NOT NULL DEFAULT '',
     `status` ENUM('active', 'inactive') NOT NULL DEFAULT 'active',
     `description` TEXT NOT NULL DEFAULT '',
     PRIMARY KEY (sale_event_id)
) ENGINE=InnoDB;

-- sale event details
CREATE TABLE IF NOT EXISTS `detail_sale_events` (
     `sale_event_id` INT UNSIGNED AUTO_INCREMENT,
     `product_id` INT UNSIGNED NOT NULL,
     `discount_type` ENUM('percent', 'amount') NOT NULL,
     `discount_percent` DECIMAL(3,3) NOT NULL DEFAULT 0,
     `discount_amount` DECIMAL(8,3) NOT NULL DEFAULT 0,
     `max_discount_price` DECIMAL(8,3) NOT NULL DEFAULT 0,
     `min_price_to_use` DECIMAL(8,3) NOT NULL DEFAULT 0,
          PRIMARY KEY(`product_id`, `sale_event_id`),
     FOREIGN KEY (`sale_event_id`) REFERENCES sale_events(`sale_event_id`),
     FOREIGN KEY (`product_id`) REFERENCES products(`product_id`),
     CONSTRAINT check_discount_type CHECK
     (
               (`discount_type` = 'percent' AND discount_percent != 0 AND discount_amount = 0) 
          OR
               (`discount_type` = 'amount' AND discount_percent = 0 AND discount_amount != 0)
     )
) ENGINE=InnoDB;

-- platform rules
CREATE TABLE IF NOT EXISTS `platform_rules` (
     `platform_rule_id` INT UNSIGNED AUTO_INCREMENT,
     `fee` DECIMAL(3, 3) NOT NULL DEFAULT 0,
     `pending_time` INT NOT NULL DEFAULT 0,
     `status` ENUM('active', 'inactive') DEFAULT 'active' NOT NULL,
     PRIMARY KEY(`platform_rule_id`)
) ENGINE = InnoDB;

ALTER TABLE `platform_rules`
ADD COLUMN `status` ENUM('active', 'inactive') NOT NULL DEFAULT 'active';

-- ======================================================== INVOICES ========================================================
-- invoices
CREATE TABLE IF NOT EXISTS `invoices` (
     `invoice_id` INT UNSIGNED AUTO_INCREMENT,
     `customer_id` INT UNSIGNED NOT NULL,
     `user_payment_method_id` INT UNSIGNED NOT NULL,
     `total_price` DECIMAL(12, 3) NOT NULL,
     `invoice_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `status` ENUM('return', 'success', 'cancel') NOT NULL DEFAULT 'success',
     PRIMARY KEY(`invoice_id`),
     FOREIGN KEY(`customer_id`) REFERENCES customers(`customer_id`),
     FOREIGN KEY(`user_payment_method_id`) REFERENCES user_payment_methods(`user_payment_method_id`) 
) ENGINE=InnoDB;

-- detail invoices
CREATE TABLE IF NOT EXISTS `detail_invoices` (
     `invoice_id` INT UNSIGNED NOT NULL,
     `product_id` INT UNSIGNED NOT NULL,
     `quantity` INT NOT NULL DEFAULT 1,
     `price` DECIMAL(12, 3) NOT NULL,
     `status` ENUM('return', 'success') NOT NULL DEFAULT 'success',
     PRIMARY KEY(`product_id`, `invoice_id`),
     FOREIGN KEY(`invoice_id`) REFERENCES invoices(`invoice_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`)
) ENGINE=InnoDB;

-- detail invoices
CREATE TABLE IF NOT EXISTS `discount_code_of_invoices` (
     `invoice_id` INT UNSIGNED NOT NULL,
     `discount_code` VARCHAR(255) NOT NULL UNIQUE,
     PRIMARY KEY(`invoice_id`, `discount_code`),
     FOREIGN KEY(`invoice_id`) REFERENCES invoices(`invoice_id`),
     FOREIGN KEY(`discount_code`) REFERENCES sale_events(`discount_code`)
) ENGINE=InnoDB;

-- transactions
CREATE TABLE IF NOT EXISTS `transactions` (
    `transaction_id` INT UNSIGNED AUTO_INCREMENT,
    `wallet_id` INT UNSIGNED NOT NULL,
    `invoice_id` INT UNSIGNED NOT NULL,
    `platform_rule_id` INT UNSIGNED DEFAULT NULL,
    `transaction_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `transaction_type` ENUM('return','purchase', 'sell') NOT NULL DEFAULT 'purchase',
    `current_balance` DECIMAL(12, 3) NOT NULL DEFAULT 0,
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
     `content` TEXT NOT NULL DEFAULT '',
     `upload_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `last_update_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     `content_rate` DECIMAL(2, 1) NOT NULL COMMENT 'rating this review from 0 to 5.0, in 0.5 increments' DEFAULT 0.0,
     `review_rate` DECIMAL(2, 1) NOT NULL COMMENT 'rating this product from 0 to 5.0, in 0.5 increments' DEFAULT 0.0,
     CONSTRAINT chk_content_rating_range CHECK (content_rate >= 0.0 AND content_rate <= 5.0),
     CONSTRAINT chk_review_rating_range CHECK (review_rate >= 0.0 AND review_rate <= 5.0), 
     PRIMARY KEY(`review_id`),
     FOREIGN KEY(`product_id`) REFERENCES products(`product_id`),
     FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE=InnoDB;

-- reply reviews
CREATE TABLE IF NOT EXISTS `reply_reviews` (
     `reply_id` INT UNSIGNED AUTO_INCREMENT,
     `review_id` INT UNSIGNED NOT NULL,
     `user_id` INT UNSIGNED NOT NULL,
     `content` TEXT NOT NULL DEFAULT '',
     `upload_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
     `last_update_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     `content_rate` DECIMAL(2, 1) NOT NULL COMMENT 'rating this review from 0 to 5.0, in 0.5 increments' DEFAULT 0,
     CONSTRAINT chk_rating_range CHECK (content_rate >= 0.0 AND content_rate <= 5.0),
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
    `start_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `last_message_time` DATETIME DEFAULT NULL,
    `status` ENUM('active', 'archived') NOT NULL DEFAULT 'active',
	PRIMARY KEY(`conversation_id`),
    FOREIGN KEY (`first_user_id`) REFERENCES users(user_id),
    FOREIGN KEY (`second_user_id`) REFERENCES users(user_id)
) ENGINE=InnoDB;

-- conversation's participants
CREATE TABLE IF NOT EXISTS `conversation_participants` (
    `conversation_id` INT UNSIGNED NOT NULL,
    `user_id` INT UNSIGNED NOT NULL,
    `last_read_time` DATETIME DEFAULT NULL,
    `join_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY(`conversation_id`, `user_id`),
    FOREIGN KEY (`conversation_id`) REFERENCES conversations(`conversation_id`) ON DELETE CASCADE,
    FOREIGN KEY (`user_id`) REFERENCES users(`user_id`)
) ENGINE=InnoDB;

-- conversation's messages
CREATE TABLE IF NOT EXISTS `messages` (
    `message_id` INT UNSIGNED AUTO_INCREMENT,
    `replied_message_id` INT UNSIGNED DEFAULT NULL,
    `send_user_id` INT UNSIGNED NOT NULL,
    `conversation_id` INT UNSIGNED NOT NULL,
    `content` TEXT NOT NULL DEFAULT '',
    `send_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `message_type` ENUM('text', 'image', 'video', 'file') NOT NULL DEFAULT 'text',
    `attachment_url` VARCHAR(255) NOT NULL DEFAULT '',
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
