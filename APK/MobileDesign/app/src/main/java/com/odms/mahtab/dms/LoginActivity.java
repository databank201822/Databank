package com.odms.mahtab.dms;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.odms.mahtab.dms.Controller.SubRouteActivity;

public class LoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);


        Button btnLogin=(Button)findViewById(R.id.btnLogin);

        btnLogin.setOnClickListener( new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent I = new Intent(LoginActivity.this, MainActivity.class);
                startActivity(I);
            }
        });


    }
}
