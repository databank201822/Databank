package com.odms.mahtab.dms.Controller;

import android.app.ActionBar;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ImageButton;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.odms.mahtab.dms.MainActivity;
import com.odms.mahtab.dms.R;

public class SubRouteActivity extends AppCompatActivity {
    // Array of strings...

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_sub_route);


        getSupportActionBar().setDisplayOptions(ActionBar.DISPLAY_SHOW_CUSTOM);
        getSupportActionBar().setDisplayShowCustomEnabled(true);
        getSupportActionBar().setCustomView(R.layout.custom_action_bar_layout);
        View view =getSupportActionBar().getCustomView();
        TextView tvOutlet = view.findViewById(R.id.actionbar);
        tvOutlet.setText("Sub Route");

        ImageButton btnBack= (ImageButton)view.findViewById(R.id.action_back_btn);

        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        ImageButton btnHome= (ImageButton)view.findViewById(R.id.action_home_btn);

        btnHome.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent I = new Intent(SubRouteActivity.this, MainActivity.class);
                startActivity(I);
                finish();
            }
        });





        final ListView listview = (ListView) findViewById(R.id.mobile_list);
        String[] mobileArray = {"Gulshan"};

         ArrayAdapter adapter = new ArrayAdapter<String>(this,R.layout.listview_row,R.id.label, mobileArray);

        ListView listView = (ListView) findViewById(R.id.mobile_list);
        listView.setAdapter(adapter);
        listview.setOnItemClickListener(new AdapterView.OnItemClickListener() {

            @Override
            public void onItemClick(AdapterView<?> parent, final View view,int position, long id) {
                String item = (String) parent.getItemAtPosition(position);
                Intent I = new Intent(SubRouteActivity.this, OrderListActivity.class);
                startActivity(I);
                Toast.makeText(getApplicationContext(), item, Toast.LENGTH_SHORT).show();
            }});
    }


}