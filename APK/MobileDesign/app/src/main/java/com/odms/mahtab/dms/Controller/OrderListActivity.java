package com.odms.mahtab.dms.Controller;

import android.app.ActionBar;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.odms.mahtab.dms.Adapter.OrderOutletListAdapter;
import com.odms.mahtab.dms.MainActivity;
import com.odms.mahtab.dms.Model.M_OrderOutlet;
import com.odms.mahtab.dms.R;

import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

import com.odms.mahtab.dms.Database.Local.order_outlet_local;
import com.odms.mahtab.dms.Database.Server.order_outlet_Server;
public class OrderListActivity extends AppCompatActivity {

    ListView listView;
    List<M_OrderOutlet> outletArrayList=new ArrayList<>();

    // Search EditText
    EditText inputSearch;
    OrderOutletListAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_order_list);

        getSupportActionBar().setDisplayOptions(ActionBar.DISPLAY_SHOW_CUSTOM);
        getSupportActionBar().setDisplayShowCustomEnabled(true);
        getSupportActionBar().setCustomView(R.layout.custom_action_bar_layout);
        View view =getSupportActionBar().getCustomView();
        TextView tvOutlet = view.findViewById(R.id.actionbar);
        tvOutlet.setText("Order Outlet List");

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
                Intent I = new Intent(OrderListActivity.this, MainActivity.class);
                startActivity(I);
                finish();
            }
        });


        inputSearch = (EditText) findViewById(R.id.inputSearch);
        inputSearch.setSelected(false);

        order_outlet_local order_outlet_local = new order_outlet_local(getApplicationContext());
        order_outlet_Server order_outlet_Server = new order_outlet_Server(getApplicationContext());
        listView = (ListView) findViewById(R.id.outlet_list_view);
       // getAllMsg();

        ListViewShow();
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long l) {


               Intent I = new Intent(OrderListActivity.this, OrderActivity.class);
               startActivity(I);


            }
        });


        inputSearch.addTextChangedListener(new TextWatcher() {

            @Override
            public void afterTextChanged(Editable arg0) {
                // TODO Auto-generated method stub
                String text = inputSearch.getText().toString().toLowerCase(Locale.getDefault());
                adapter.filter(text);
            }

            @Override
            public void beforeTextChanged(CharSequence arg0, int arg1,int arg2, int arg3) {
                // TODO Auto-generated method stub
            }

            @Override
            public void onTextChanged(CharSequence arg0, int arg1, int arg2,
                                      int arg3) {
                // TODO Auto-generated method stub
            }
        });


    }


    void insertOrderOutlet( int outletId, int outletCode,  String outletName,int routeId, String orderCS,int orderStatus){
        order_outlet_Server DBin = new order_outlet_Server(getApplicationContext());
        DBin.insertOrderOutlet(new M_OrderOutlet(outletId,outletCode,outletName,routeId,orderCS,orderStatus));
    }

    public void getAllMsg(){
        for(int i=1;i<50;i++) {
            if(i%4>1) {
                insertOrderOutlet(i,30000+i,"Outlet"+i,1,"3.40",1);
            }else{
                insertOrderOutlet(i,30000+i,"Outlet"+i,1,"3.40",0);
            }

        }

    }
    void ListViewShow(){

        outletArrayList=new ArrayList<>();
        order_outlet_local order_outlet_local = new order_outlet_local(getApplicationContext());
        List<M_OrderOutlet> Conts = order_outlet_local.getalloutlet();

        for (M_OrderOutlet Cont : Conts) {
            outletArrayList.add(new M_OrderOutlet(Cont.get_id(),Cont.get_outletId(),Cont.get_outletCode(),Cont.get_outletName(),Cont.get_routeId(),Cont.get_orderCS(),Cont.get_orderStatus()));
            Log.e("getAllMsg","Message function"+Cont.get_orderStatus());
        }

        adapter = new OrderOutletListAdapter(this, R.layout.order_outlet_listview_row, outletArrayList);
        listView.setAdapter(adapter);

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int position, long l) {



            }
        });
    }


}


