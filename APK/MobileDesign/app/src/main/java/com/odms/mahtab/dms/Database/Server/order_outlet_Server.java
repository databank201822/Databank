package com.odms.mahtab.dms.Database.Server;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;

import com.odms.mahtab.dms.Database.Database;
import com.odms.mahtab.dms.Model.M_OrderOutlet;

import java.util.ArrayList;
import java.util.List;

public class order_outlet_Server {


    Context Contex;

    SQLiteDatabase db;

    public order_outlet_Server(Context context) {
        Database LocDB = new Database(context);
        db = LocDB.getReadableDatabase();

    }//select DB


    public void insertOrderOutlet(M_OrderOutlet outlet) {
        ContentValues values = new ContentValues();
        values.put("outletId", outlet.get_outletId()); // outletid
        values.put("outletCode", outlet.get_outletCode()); //
        values.put("outletName", outlet.get_outletName()); //
        values.put("routeId",outlet.get_routeId()); //
        values.put("orderCS", outlet.get_orderCS()); //
        values.put("orderStatus",outlet.get_orderStatus()); //

        db.insert("tbld_outlet", null, values);
        Log.e("InsertIntotbld_outlet",""+values.toString());
        db.close();
    }


    public void deleteAllMsg() {

        db.delete("tbld_outlet", null, null);
        db.close();
    }

}
