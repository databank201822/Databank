package com.odms.mahtab.dms.Database;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

public class Database extends SQLiteOpenHelper {

    private static final int DATABASE_VERSION = 1;                     //DATABASE_VERSION
    private static final String DATABASE_NAME = "ODMS";  //DATABASE_NAME
    Context Contex;

    public Database(Context context) {

        super(context, DATABASE_NAME, null, DATABASE_VERSION);

    }//select DB

    @Override
    public void onCreate(SQLiteDatabase db) {

        String tbld_outlet = "CREATE TABLE tbld_outlet (id INTEGER PRIMARY KEY,outletId INTEGER,outletCode INTEGER,outletName TEXT,routeId INTEGER ,orderCS TEXT,orderStatus INTEGER)";
        Log.e("tbl_order_outlet", tbld_outlet);
        db.execSQL(tbld_outlet);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("DROP TABLE IF EXISTS tbld_outlet");
        onCreate(db);
    }

}

