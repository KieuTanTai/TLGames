using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace TLGames.Infrastructure.Configuration
{
    public static class SqlWhitelist
    {
        private static readonly HashSet<string> AllowedColumnNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        // ROLES
        "role_id", "role_name", "status",

        // USERS
        "user_id", "user_name", "password", "create_date", "status",

        // USER STORAGES
        "user_storage_id",

        // WALLETS
        "wallet_id", "balance", "currency", "last_update_balance_date",

        // USER RELATIONSHIPS
        "requester_id", "receiver_id", "request_date", "accept_date",

        // ROLE OF USERS
        "create_date",

        // CUSTOMERS
        "customer_id", "age", "personal_phone", "personal_name", "personal_address", "avatar_url", "background_url", "biology",

        // CARTS
        "cart_id", "total_price",

        // DEVELOPERS
        "developer_id", "developer_name", "became_developer_date", "description", "website_url", "studio_phone", "studio_address", "studio_email",

        // FOLLOWER OF DEVELOPERS
        "follower_id", "start_follow_date",

        // PUBLISHERS
        "publisher_id", "publisher_name", "became_publisher_date", "business_phone", "business_address", "business_email",

        // PRODUCTS
        "product_id", "product_name", "release_date", "base_price", "trailer_url", "game_mode", "rating_age", "downloaded_count",

        // PRODUCT IMAGES
        "product_image_id", "image_url",

        // PRODUCT VERSIONS
        "product_version_id", "executable_path", "download_url", "upload_date", "version", "launch_args", "size_mb", "update_description",

        // CATEGORIES
        "category_id", "category_name",

        // DETAIL USER STORAGES
        "last_played", "play_time", "is_favored", "purchase_date", "is_installed", "installed_date",

        // DETAIL CARTS
        "add_date", "price", "type",

        // NEWS CATEGORIES
        "news_category_id",

        // NEWS
        "news_id", "related_product_id", "title", "published_date", "content",

        // NEWS IMAGES
        "news_image_id",

        // BANKS
        "bank_id", "bank_name",

        // USER PAYMENT METHODS
        "user_payment_method_id", "payment_method_type", "added_date", "last_updated_date", "display_name", "last_four_digit", "expiry_year", "expiry_month", "token",

        // SALE EVENTS
        "sale_event_id", "discount_code", "start_date", "end_date", "sale_event_name", "description",

        // DETAIL SALE EVENTS
        "discount_type", "discount_percent", "discount_amount", "max_discount_price", "min_price_to_use",

        // PLATFORM RULES
        "platform_rule_id", "fee", "pending_time",

        // INVOICES
        "invoice_id", "total_price", "invoice_date",

        // DETAIL INVOICES
        "quantity",

        // TRANSACTIONS
        "transaction_id", "transaction_date", "transaction_type", "current_balance",

        // REVIEWS
        "review_id", "upload_date", "last_update_date", "content_rate", "review_rate",

        // REPLY REVIEWS
        "reply_id",

        // CONVERSATIONS
        "conversation_id", "first_user_id", "second_user_id", "start_time", "last_message_time",

        // CONVERSATION PARTICIPANTS
        "last_read_time", "join_time",

        // MESSAGES
        "message_id", "replied_message_id", "send_user_id", "content", "send_time", "message_type", "attachment_url"
    };

        // Regex kiểm tra tên cột hợp lệ về mặt cú pháp
        private static readonly Regex ColumnNameRegex = new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$", RegexOptions.Compiled);

        public static bool IsSafeColumn(string colName)
        {
            return ColumnNameRegex.IsMatch(colName) && AllowedColumnNames.Contains(colName);
        }
    }
}
