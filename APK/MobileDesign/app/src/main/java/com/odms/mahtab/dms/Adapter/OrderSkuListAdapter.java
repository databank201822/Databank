package com.odms.mahtab.dms.Adapter;

import android.annotation.SuppressLint;
import android.content.Context;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.odms.mahtab.dms.Model.M_OrderOutlet;
import com.odms.mahtab.dms.R;

import java.util.List;

/**
 * Created by Mahtab on 08-Feb-2018.
 */

public class OrderSkuListAdapter extends ArrayAdapter<M_OrderOutlet> {

    List<M_OrderOutlet> outletList;

    //activity context
    Context context;

    //the layout resource file for the list items
    int resource;


    public OrderSkuListAdapter(Context Context, int resource, List<M_OrderOutlet> outletList){
        super(Context, resource, outletList);
        this.context = Context;
        this.resource = resource;
        this.outletList = outletList;
    }

    // return position here
    @Override
    public long getItemId(int position) {
        return position;
    }
    // return size of list
    @Override
    public int getCount() {
        return outletList.size();
    }
    //get Object from each position
    @Override
    public M_OrderOutlet getItem(int position) {
        return outletList.get(position);
    }



    @SuppressLint("ResourceAsColor")
    @NonNull
    @Override
    public View getView(int position, @Nullable View convertView, @NonNull ViewGroup parent) {


        LayoutInflater layoutInflater = LayoutInflater.from(context);
        View view = layoutInflater.inflate(resource, null, false);

        TextView tvOutlet = view.findViewById(R.id.tvOutletname);
        TextView tvCode = view.findViewById(R.id.tvCode);
        TextView tvOrderCS = view.findViewById(R.id.tvOrderCS);
        TextView tvStatus = view.findViewById(R.id.tvStatus);
        LinearLayout ListItemLayout =view.findViewById(R.id.ListItemLayout);


        M_OrderOutlet message=outletList.get(position);
        if(message.get_orderStatus()==0) {

        }else if(message.get_orderStatus()==1) {
           // ListItemLayout.setBackgroundColor(R.color.colorPrimary);
            tvOrderCS.setText(message.get_orderCS()+" CS");
        }else if(message.get_orderStatus()==2) {
            // ListItemLayout.setBackgroundColor(R.color.colorPrimary);
        }
        tvOutlet.setText(message.get_outletName());
        tvCode.setText(String.valueOf(message.get_outletCode()));

     //   tvStatus.setText(String.valueOf(message.get_orderStatus()));

        return view;
    }
    // Defined Array values to show in ListView



}
