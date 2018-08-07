package com.odms.mahtab.dms.Database.Local;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;

import com.odms.mahtab.dms.Database.Database;
import com.odms.mahtab.dms.Model.M_OrderOutlet;

import java.util.ArrayList;
import java.util.List;

public class order_outlet_local {


    Context Contex;

    SQLiteDatabase db;

    public order_outlet_local(Context context) {
        Database LocDB = new Database(context);
        db = LocDB.getReadableDatabase();

    }//select DB




    public List<M_OrderOutlet> getalloutlet() {

        List<M_OrderOutlet> AllOutletlist = new ArrayList<>();

        String selectQuery = "SELECT * FROM tbld_outlet";

        Cursor cursor = db.rawQuery(selectQuery, null);
        // looping through all rows and adding to list
        if (cursor.moveToFirst()) {
            do {
                M_OrderOutlet Outlet = new M_OrderOutlet();
                Outlet.set_id(Integer.parseInt(cursor.getString(0)));
                Outlet.set_outletId(Integer.parseInt(cursor.getString(1)));
                Outlet.set_outletCode(Integer.parseInt(cursor.getString(2)));
                Outlet.set_outletName(cursor.getString(3));
                Outlet.set_routeId(Integer.parseInt(cursor.getString(4)));
                Outlet.set_OrderCS(cursor.getString(5));
                Outlet.set_orderStatus(Integer.parseInt(cursor.getString(6)));


                // Adding contact to list
                AllOutletlist.add(Outlet);
            } while (cursor.moveToNext());
        }
        db.close();
        return AllOutletlist;

    }

}
