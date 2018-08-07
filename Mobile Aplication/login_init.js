function make_tables() {
    document.addEventListener("deviceready", onDeviceReady, true);

    function onDeviceReady() {
        //	 navigator.notification.alert("PhoneGap is working!!");
        var db = window.openDatabase("sales_distribution", "1.0", "sales_distribution", 2000000);

        db.transaction(populateDB, errorCB, successCB);
        //   alert("-------------------------------------");
    }

    function populateDB(tx) {
        //   console.log("_000000000000000000000000");
        tx.executeSql('DROP TABLE IF EXISTS tbli_focus_sell_sku;');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_focus_sell_sku(id, sku_id)');


        // tx.executeSql('DROP TABLE IF EXISTS tbli_sync_sales_order;');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_sync_sales_order(id INTEGER PRIMARY KEY AUTOINCREMENT,local_so_id,server_so_id,sr_id,sales_order_date,is_synced)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_sync_damage_order(id INTEGER PRIMARY KEY AUTOINCREMENT,local_do_id,server_do_id,sr_id,sales_order_date,is_synced)');


        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_save_log(id INTEGER PRIMARY KEY AUTOINCREMENT,save_date_time)');

        tx.executeSql('DROP TABLE IF EXISTS tbld_route_new');
        tx.executeSql('DROP TABLE IF EXISTS tbld_market_new');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_route_new (outlet_id, outlet_code, outlet_name, parent_id, sequence_no, is_location_verified) ');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_market_new (route_id,route_code, route_name, market_id, market_name) ');

        tx.executeSql(" DROP TABLE IF EXISTS tbld_spot_sales_route ");
        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_spot_sales_route (route_id, route_name) ");

        tx.executeSql('DROP TABLE IF EXISTS tbld_outlet_category');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet_category(id INTEGER PRIMARY KEY AUTOINCREMENT, outlet_category_id, outlet_category_name)');

        tx.executeSql('DROP TABLE IF EXISTS tbld_outlet_grade');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet_grade(id INTEGER PRIMARY KEY AUTOINCREMENT, outlet_grade_id, outlet_grade_name, outlet_grade_code)');

        tx.executeSql('DROP TABLE IF EXISTS tbld_outlet_channel');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet_channel(id INTEGER PRIMARY KEY AUTOINCREMENT, outlet_channel_id, outlet_channel_name, outlet_channel_code)');

        tx.executeSql(" DROP TABLE IF EXISTS tbld_competitor ");
        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_competitor (com_id, com_name, com_code) ");

        tx.executeSql(" DROP TABLE IF EXISTS tbli_target_report ");
        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbli_target_report (id INTEGER PRIMARY KEY AUTOINCREMENT, sr_id, month, year, target, achieve) ");

        tx.executeSql(" DROP TABLE IF EXISTS tbld_competitor_sku ");
        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_competitor_sku  (com_sku_id, com_sku_name, com_sku_code, com_sku_parent_id) ");


        tx.executeSql(" DROP TABLE IF EXISTS tbld_competitor_sku_hierarchy_elements ");
        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_competitor_sku_hierarchy_elements " +
                " (com_sku_hierarchy_id, com_sku_hierarchy_name, com_sku_hierarchy_category_id, com_sku_hierarchy_parent_element_id, company_id) ");

        tx.executeSql(" DROP TABLE IF EXISTS tbli_market_insight_tertiary_sku_mapping ");
        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbli_market_insight_tertiary_sku_mapping (id INTEGER PRIMARY KEY AUTOINCREMENT, program_name, sku_id, sku_name, company_id, sys_date) ");


        tx.executeSql(" CREATE TABLE IF NOT EXISTS tblt_market_insight_tertiary_tracking (id INTEGER PRIMARY KEY AUTOINCREMENT, date, db_id, sr_id, route_id, market_id, outlet_id, company_id, brand_id, sku_id, opening_stock, purchase_stock, closing_stock, is_synced) ");
        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbli_market_insight_image_upload(id INTEGER PRIMARY KEY AUTOINCREMENT , outlet_id, outlet_image, is_synced) ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbli_market_insight_tracking(id INTEGER PRIMARY KEY AUTOINCREMENT,dbhouse_id, emp_id, route_id, mkt_id, outlet_id, company_id, sku_hierarchy_id, sku_id, case_qty, pcs_qty, outlet_img_loc, is_synced) ");


        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet_verification(id INTEGER PRIMARY KEY AUTOINCREMENT, outlet_id, lat, lon, local_img_loc, is_synced,verify_date_time)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet_verification_image_upload(id INTEGER PRIMARY KEY AUTOINCREMENT, outlet_id, local_img_loc, is_synced)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_new_outlet(id INTEGER PRIMARY KEY AUTOINCREMENT,emp_id, dbhouse_id, outlet_name, outlet_code, outlet_address, outlet_owner, owner_contact, outlet_category, outlet_grade, outlet_channel, outlet_type, outlet_visi_cooler, mkt_id, route_id, outlet_lat, outlet_lon, outlet_image, is_synced)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_new_outlet_image_upload(id INTEGER PRIMARY KEY AUTOINCREMENT , outlet_code, outlet_image, is_synced)');

        tx.executeSql('DROP TABLE IF EXISTS tbld_sku_hierarchy_elements');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_sku_hierarchy_elements (id PRIMARY KEY, element_name, element_code, element_description, element_category_id, parent_element_id, image)');

        tx.executeSql('DROP TABLE IF EXISTS tbls_available_inventory');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbls_available_inventory (id INTEGER PRIMARY KEY AUTOINCREMENT, distribution_id, sku_id, quantity, foc_quantity)');

        tx.executeSql('DROP TABLE IF EXISTS tblh_available_inventory_details');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblh_available_inventory_details (id, distribution_id, batch_no, challan_date, gr_date, price, sku_id, quantity, foc_quantity, batch_active)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tblts_sales_order(id INTEGER PRIMARY KEY AUTOINCREMENT, so_id, outlet_id, planned_order_date, order_date_time, shipping_date, delivery_date, sr_id, db_id, so_status, total_order, total_due, total_confirmed, total_delivered, cash_received, sales_order_type_id, manual_discount, is_synced, route_id, market_id)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_sales_order_line(id INTEGER PRIMARY KEY   AUTOINCREMENT, so_id, so_line_id, sku_id, sku_order_type_id, promotion_id, valid_line_item, quantity_ordered, quantity_confirmed, quantity_delivered, unit_sale_price, total_sale_price, total_discount_amount, total_billed_amount)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_damage_order(do_id PRIMARY KEY, outlet_id, sr_id, db_id, order_date, delivery_date, is_synced)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_damage_order_line(do_id, sku_id, do_status, damage_order_type_id, quantity_order, quantity_confirm, quantity_delivered, image_name, local_image_path)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_damage_order_image_upload(do_id, sku_id, image_name, local_image_path, isSynced)');


        tx.executeSql('DROP TABLE IF EXISTS tblt_delivery_order');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_delivery_order(id INTEGER PRIMARY KEY AUTOINCREMENT, so_id, route_id, market_id, outlet_id, planned_order_date, order_date_time, shipping_date, delivery_date, sr_id, db_id, so_status, total_order, total_due, total_grb, total_grb_due, total_crate, total_crate_due, total_confirmed, total_delivered, cash_received, grb_received, crate_received, sales_order_type_id, manual_discount, outlet_name, outlet_code, prom_delete, is_synced)');

        tx.executeSql('DROP TABLE IF EXISTS tblt_delivery_order_line');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_delivery_order_line(id INTEGER PRIMARY KEY AUTOINCREMENT, so_id, sku_id, sku_order_type_id, promotion_id, valid_line_item, quantity_ordered, quantity_confirmed, quantity_delivered, quantity_grb_ordered, quantity_grb_confirmed, quantity_grb_delivered, quantity_crate_ordered, quantity_crate_confirmed, quantity_crate_delivered, unit_sale_price, total_sale_price, total_discount_amount, total_billed_amount, prom_delete )');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_visicooler_data(id INTEGER PRIMARY KEY AUTOINCREMENT, db_id, sr_id, route_id, market_id, outlet_id, purity, charging, visicooler_image_name,so_id, is_synced )');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_visicooler_image_upload(id INTEGER PRIMARY KEY AUTOINCREMENT, outlet_id, image_name, local_image_path, isSynced)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet_details(outlet_id, outlet_name, outlet_code, address, lat, lon, owner_name owner_phone_number, outlet_due, outlet_grb, outlet_crate, outlet_vesicular)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_outlet_previous_sales(outlet_id, so_id, order_date, total_ordered, total_order_sku, total_deliver_sku, total_bought, total_case, total_pcs)');

        tx.executeSql('DROP TABLE IF EXISTS tbld_exception_reason');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_exception_reason(exception_reason_id PRIMARY KEY, exception_reason_code, exception_reason_name)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblts_outlet_visit_status(id INTEGER PRIMARY KEY AUTOINCREMENT, outlet_id, visit_date, sales_order_num, distance_flag, distance, exception_id, remarks, market_id, start_time, end_time, is_synced)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_order_inventory(dbhouse_id, sku_id, units_ordered)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_sku_new (sku_id, sku_code, sku_name, sku_default_price, sku_default_mou, default_qty, parent_id, sku_container_type_code, sku_volume, sku_lpc)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_sku_mou_mapping_new (sl_no, sku_id, sku_code, sku_name, mou_id, mou_name, qty, price)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_unprocessed_sales_order(id INTEGER PRIMARY KEY AUTOINCREMENT, so_id, outlet_code, outlet_name, outlet_due, order_date, delivery_date, total, cash, so_status, sku_id, actual_price, discount, subtotal, quantity_ordered, quantity_delivered, is_synced)');
        //tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_unprocessed_sales_order_line(id INTEGER PRIMARY KEY AUTOINCREMENT, so_id, sku_id, quantity, actual_price, discount_id, subtotal, quantity_delivered)');


        tx.executeSql('DROP TABLE IF EXISTS tbld_trade_promotion;');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_trade_promotion(trade_promotion_id, trade_promotion_name, trade_promotion_description, trade_promotion_start_date, trade_promotion_end_date)');


        tx.executeSql('DROP TABLE IF EXISTS tbld_promotion_details');
        tx.executeSql('DROP TABLE IF EXISTS tbld_promotion_outlet_mapping');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_promotion_details (promo_id, promo_name, promo_type, sku_id, sku_code, unit, unit_qty, qty, discount_type, percentage_discount, unit_price_discount, total_price_discount, promo_sku_code, promo_sku_price, free_sku_name, free_sku_unit, free_sku_qty, free_unit_qty)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_promotion_outlet_mapping (promo_id, promo_outlet_id, promo_outlet_code)');


        tx.executeSql(" DROP TABLE IF EXISTS tbld_slab_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_n_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_n_promotion_definition ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_promotion_outlet_mapping ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_slab_promotion " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, name, code, description, promo_type, promotion_unit_id, promotion_sub_unit_id, start_date, end_date, promo_expiry_date, is_active) ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_n_promotion " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, promo_id, promo_name, promo_code, promo_text, promo_type, promo_offer_type) ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_n_promotion_definition " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, promo_id, rule_entry_type, condition_unit_type, condition_sku_id, condition_sku_amount INTEGER, offer_sku_id, offer_sku_amount, condition_bundle_min_amount,group_id) ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbli_promotion_outlet_mapping " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, promo_id, outlet_id) ");


        tx.executeSql(" DROP TABLE IF EXISTS tbld_dm_slab_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_dm_n_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_dm_n_promotion_definition ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_dm_promotion_outlet_mapping ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_dm_slab_promotion " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, name, code, description, promo_type, promotion_unit_id, promotion_sub_unit_id, start_date, end_date, promo_expiry_date, is_active) ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_dm_n_promotion " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, promo_id, promo_name, promo_code, promo_text, promo_type, promo_offer_type) ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbld_dm_n_promotion_definition " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, promo_id INTEGER, rule_entry_type, condition_unit_type, condition_sku_id, condition_sku_amount, offer_sku_id, offer_sku_amount, condition_bundle_min_amount,group_id) ");

        tx.executeSql(" CREATE TABLE IF NOT EXISTS tbli_dm_promotion_outlet_mapping " +
                "(id INTEGER PRIMARY KEY AUTOINCREMENT, promo_id, outlet_id) ");



        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_sku(id PRIMARY KEY, code, name, logical_type, node_type, parent_code, parent_name, sku_model, sku_brand, sku_manufacturer, sku_product_line, sku_category, sku_micro_category, sku_macro_category, sku_commercial_category,sku_color,sku_size,sku_shape,sku_weight,sku_default_price,sku_default_mou,mou_qty)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_sku_mou_mapping(sku,mou,qty,qty_mou,price,default_mou)');
        //tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_bundle_sku_sku_mapping(bundle_sku_name, bundle_sku_code,child_sku,child_sku_qty)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_sku_outlet_channel_mapping(sku_name, sku_code, outlet_channel_code)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbli_outlet_channel_outlet_mapping(outlet_code, outlet_channel_code)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_sku_category (sku_name, sku_code, category)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_outlet_category (outlet, category)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_mi_sku (name_mi_sku,code_mi_sku,brand_mi_sku,competitor_code)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_mi_brand (name_mi_brand,code_mi_brand,parent_mi_brand)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_mi_product (name_mi_product,code_mi_product,parent_mi_product)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_mi_product_line (name_mi_product_line,code_mi_product_line)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_mi_promotion (name_mi_promotion,code_mi_promotion)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_competitors (name_competitor,code_competitor)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_comp_merchandising_info (name_comp_merchandising,code_comp_merchandising)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_share_tracking_plan (outlet_code, outlet_name , code, name, brand_code, brand_name, product_code, product_name, pl_code, pl_name)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_market_insight_activity (outlet, competitor, sku, merchandising_info, promotion, comment)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_market_insight_share (outlet, competitor, sku, sold_qty, sold_value, comment)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_market_insight_inventory (outlet_code,outlet_name, sku_code,sku_name,stock)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_market (market_code, market_name)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet(outlet_code PRIMARY KEY, outlet_name, outlet_category, market_code, owner_id, owner_contact, outlet_address, lat, lon, is_location_verified, pic)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_outlet_avg_sale (outlet,sku_category,avg_amount)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_outlet_due(outlet_name, outlet_code, due_amount)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet_pic (outlet_code, pic)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_outlet_count_last_month (date, outlet_count)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbl_new_outlet (mkt, outlet_name, outlet_code, outlet_address, outlet_owner, owner_contact, outlet_category, lat, lon, pic, is_synced)');
        //   tx.executeSql('CREATE TABLE IF NOT EXISTS tblp_sales_order_serial(so_serial PRIMARY KEY)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_stock_rule(sku,check_method,check_rule,lowest_limit,highest_limit)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_inventory(sku, qty)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_outlet_inventory(outlet_code,outlet_name, sku_code,sku_name,qty)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS sku_qty_count(sku, qty, subtotal)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_route(route,outlet_name,outlet_code,sequence)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_all_routes(route,outlet_name,outlet_code,sequence,activity_date)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_damage_criteria(damage_criteria_code PRIMARY KEY, damage_criteria_name, damage_category)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_message (msg, subject, time_stamp, frm, read_flag)');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_message_saved (msg, subject, time_stamp, sent_to)');

        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_thana(thana_code  PRIMARY KEY, thana_name)');
        tx.executeSql('DROP TABLE IF EXISTS tbld_distribution_employee;');
        tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_distribution_employee(emp_id, name, designation, contact_no, email, address_name, dbhouse_name, user_id, image,db_manager)');


        tx.executeSql('CREATE TABLE IF NOT EXISTS tblt_day_end(id INTEGER PRIMARY KEY AUTOINCREMENT, user_id, dbhouse, dist_emp_id,day_end_date_time,is_synced)');
        //   tx.executeSql('INSERT INTO sku_qty_count (sku, qty, subtotal) VALUES ("ponds","10","500")');

        //   tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_dbhouse_stock_rule(dbhouse PRIMARY KEY, lowest_limit_percentage,highest_limit_percentage,lowest_limit_unit,highest_limit_unit,lowest_limit_value,highest_limit_value)');
        //   tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_dbhouse_sku_stock_rule(dbhouse,sku_code,lowest_limit_percentage,highest_limit_percentage,lowest_limit_unit,highest_limit_unit,lowest_limit_value,highest_limit_value)');
        //   tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_dbhouse_sr_stock_rule(dbhouse,sr,lowest_limit_percentage,highest_limit_percentage,lowest_limit_unit,highest_limit_unit,lowest_limit_value,highest_limit_value)');
        //   tx.executeSql('CREATE TABLE IF NOT EXISTS tbld_dbhouse_sr_sku_stock_rule(dbhouse,sr,sku_code,lowest_limit_percentage,highest_limit_percentage,lowest_limit_unit,highest_limit_unit,lowest_limit_value,highest_limit_value)');
        //   tx.executeSql("INSERT INTO tbld_sku (code, name, discount, actual_price, sku_status) VALUES ('p1','ponds','1','50','1')");
        //   tx.executeSql("INSERT INTO tbld_sku (code, name, discount, actual_price, sku_status) VALUES ('p2','ponds2','1','50','1')");
        //   tx.executeSql("INSERT INTO tbld_sku (code, name, discount, actual_price, sku_status) VALUES ('t1','tab','200','1000','available')");
        //   tx.executeSql("INSERT INTO tbld_outlet (outlet_id,outlet_name,outlet_code,owner_id,outlet_address) VALUES ('out1','banani_outlet','o1','out_own1','dhaka')");
        //   tx.executeSql("INSERT INTO tblp_sales_order_serial (user_id,so_serial) VALUES ('1','10')");
    }

    function errorCB(tx, err) {
        alert("Database not created: " + err);
    }

    function successCB() {
        console.log("db creation success!");
    }

}

function apkUpdateDBProsessAndMaketable() {
    document.addEventListener("deviceready", onDeviceReady, true);

    function onDeviceReady() {
        //	 navigator.notification.alert("PhoneGap is working!!");
        var db = window.openDatabase("sales_distribution", "1.0", "sales_distribution", 2000000);
        db.transaction(deleteDB, errorCB, successCB);
        //   alert("-------------------------------------");
    }

    function deleteDB(tx) {
        //   console.log("_000000000000000000000000");
        tx.executeSql('DROP TABLE IF EXISTS tbli_focus_sell_sku;');
        tx.executeSql('DROP TABLE IF EXISTS tbli_sync_sales_order;');
        tx.executeSql('DROP TABLE IF EXISTS tbli_save_log;');
        tx.executeSql('DROP TABLE IF EXISTS tbld_route_new');
        tx.executeSql('DROP TABLE IF EXISTS tbld_market_new');
        tx.executeSql(" DROP TABLE IF EXISTS tbld_spot_sales_route ");
        tx.executeSql('DROP TABLE IF EXISTS tbld_outlet_category');
        tx.executeSql('DROP TABLE IF EXISTS tbld_outlet_grade');
        tx.executeSql('DROP TABLE IF EXISTS tbld_outlet_channel');
        tx.executeSql(" DROP TABLE IF EXISTS tbld_competitor ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_target_report ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_competitor_sku ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_competitor_sku_hierarchy_elements ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_market_insight_tertiary_sku_mapping ");
        tx.executeSql(" DROP TABLE IF EXISTS tblt_market_insight_tertiary_tracking ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_market_insight_image_upload ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_market_insight_tracking ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_outlet_verification ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_outlet_verification_image_upload ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_new_outlet ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_new_outlet_image_upload ");
        tx.executeSql('DROP TABLE IF EXISTS tbld_sku_hierarchy_elements');
        tx.executeSql('DROP TABLE IF EXISTS tbls_available_inventory');
        tx.executeSql('DROP TABLE IF EXISTS tblh_available_inventory_details');
        tx.executeSql('DROP TABLE IF EXISTS tblts_sales_order');
        tx.executeSql('DROP TABLE IF EXISTS tblt_sales_order_line');
        tx.executeSql('DROP TABLE IF EXISTS tblt_damage_order');
        tx.executeSql('DROP TABLE IF EXISTS tblt_damage_order_line');
        tx.executeSql('DROP TABLE IF EXISTS tblt_damage_order_image_upload');
        tx.executeSql('DROP TABLE IF EXISTS tblt_delivery_order');
        tx.executeSql('DROP TABLE IF EXISTS tblt_delivery_order_line');
        tx.executeSql('DROP TABLE IF EXISTS tbld_visicooler_data');
        tx.executeSql('DROP TABLE IF EXISTS tbld_visicooler_image_upload');
        tx.executeSql('DROP TABLE IF EXISTS tbld_outlet_details');
        tx.executeSql('DROP TABLE IF EXISTS tbli_outlet_previous_sales');
        tx.executeSql('DROP TABLE IF EXISTS tbld_exception_reason');
        tx.executeSql('DROP TABLE IF EXISTS tblts_outlet_visit_status');
        tx.executeSql('DROP TABLE IF EXISTS tblt_order_inventory');
        tx.executeSql('DROP TABLE IF EXISTS tbld_sku_new');
        tx.executeSql('DROP TABLE IF EXISTS tbld_sku_mou_mapping_new');
        tx.executeSql('DROP TABLE IF EXISTS tblt_unprocessed_sales_order');
        tx.executeSql('DROP TABLE IF EXISTS tbld_trade_promotion;');
        tx.executeSql('DROP TABLE IF EXISTS tbld_promotion_details');
        tx.executeSql('DROP TABLE IF EXISTS tbld_promotion_outlet_mapping');
        tx.executeSql(" DROP TABLE IF EXISTS tbld_slab_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_n_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_n_promotion_definition ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_promotion_outlet_mapping ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_dm_slab_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_dm_n_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_dm_n_promotion_definition ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_dm_promotion_outlet_mapping ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_sku ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_sku_mou_mapping ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_sku_outlet_channel_mapping ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_outlet_channel_outlet_mapping ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_sku_category ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_outlet_category ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_mi_sku ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_mi_brand ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_mi_product ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_mi_product_line ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_mi_promotion ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_competitors ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_comp_merchandising_info ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_share_tracking_plan ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_market_insight_activity ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_market_insight_share ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_market_insight_inventory ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_market ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_outlet ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_outlet_avg_sale ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_outlet_due ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_outlet_pic ");
        tx.executeSql(" DROP TABLE IF EXISTS tblt_outlet_count_last_month ");
        tx.executeSql(" DROP TABLE IF EXISTS tbl_new_outlet ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_stock_rule ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_inventory ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_outlet_inventory ");
        tx.executeSql(" DROP TABLE IF EXISTS sku_qty_count ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_route ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_all_routes ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_damage_criteria ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_message ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_message_saved ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_thana ");
        tx.executeSql(" DROP TABLE IF EXISTS tbld_distribution_employee ");
        tx.executeSql(" DROP TABLE IF EXISTS tbli_sync_damage_order ");
    }

    function errorCB(tx, err) {
        alert("Database flushed Eror login init: " + err);
    }

    function successCB() {
         make_tables();
        console.log("db flushed success!");
    }
}
