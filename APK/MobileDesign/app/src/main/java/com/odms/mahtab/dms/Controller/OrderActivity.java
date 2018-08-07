package com.odms.mahtab.dms.Controller;

import android.app.ActionBar;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.support.design.widget.TabLayout;

import android.os.Bundle;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import com.odms.mahtab.dms.Adapter.ViewPagerAdapter;
import com.odms.mahtab.dms.Fragment.Fragment_OrderFirstStep;
import com.odms.mahtab.dms.Fragment.Fragment_OrderForthStep;
import com.odms.mahtab.dms.Fragment.Fragment_OrderSecondStep;
import com.odms.mahtab.dms.Fragment.Fragment_OrderThirdStep;
import com.odms.mahtab.dms.MainActivity;
import com.odms.mahtab.dms.R;
import com.odms.mahtab.dms.Adapter.*;

public class OrderActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_order);

        getSupportActionBar().setDisplayOptions(ActionBar.DISPLAY_SHOW_CUSTOM);
        getSupportActionBar().setDisplayShowCustomEnabled(true);
        getSupportActionBar().setCustomView(R.layout.custom_action_bar_layout);
        View view =getSupportActionBar().getCustomView();
        TextView tvOutlet = view.findViewById(R.id.actionbar);
        tvOutlet.setText("Order");

        ImageButton btnBack= (ImageButton)view.findViewById(R.id.action_back_btn);
        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                AlertDialogMassage("Do You close");
            }
        });

        ImageButton homeBack= (ImageButton)view.findViewById(R.id.action_home_btn);
        homeBack.setVisibility(View.GONE);




        ViewPager viewPager = (ViewPager) findViewById(R.id.pager);
        ViewPagerAdapter adapter = new ViewPagerAdapter(getSupportFragmentManager());
        adapter.addFragment(new Fragment_OrderFirstStep(), "Outlet");
        adapter.addFragment(new Fragment_OrderSecondStep(), "Order");
        adapter.addFragment(new Fragment_OrderThirdStep(), "VisiCooler");
        adapter.addFragment(new Fragment_OrderForthStep(), "Summary");
        viewPager.setAdapter(adapter);

        TabLayout tabLayout = (TabLayout) findViewById(R.id.tabs);
        tabLayout.setupWithViewPager(viewPager);

    }

    public void AlertDialogMassage(String massage) {

        AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
        alertDialog.setTitle("ODMS");
        alertDialog.setMessage(massage);

        alertDialog.setCancelable(true);

        alertDialog.setPositiveButton("Yes", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
            finish();
            }
        });
        alertDialog.setNegativeButton("No", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {

            }
        });

        alertDialog.show();
    }
    @Override
    public void onBackPressed() {
        Toast.makeText(getApplicationContext(),"Sorry",Toast.LENGTH_LONG).show();
    }


}
