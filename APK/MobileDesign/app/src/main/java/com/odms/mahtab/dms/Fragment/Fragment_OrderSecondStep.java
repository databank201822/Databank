package com.odms.mahtab.dms.Fragment;

import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.Toast;

import com.odms.mahtab.dms.Controller.OrderSkuListActivity;
import com.odms.mahtab.dms.R;

/**
 * Created by Anu on 22/04/17.
 */



public class Fragment_OrderSecondStep extends Fragment {

    public Fragment_OrderSecondStep() {
        // Required empty public constructor
    }
    public static Fragment_OrderSecondStep newInstance() {
        Fragment_OrderSecondStep fragment = new Fragment_OrderSecondStep();
        return fragment;
    }
        @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }
    private View mRootView;
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,Bundle savedInstanceState) {
        if(mRootView==null){
            mRootView = inflater.inflate(R.layout.fragment_ordersecondstep, container, false);
            //......
        }

        Button button = (Button) mRootView.findViewById(R.id.btn_addsku);
        button.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                Toast.makeText(getActivity(),"Button Click",Toast.LENGTH_LONG).show();
                Intent intent = new Intent(getActivity(),OrderSkuListActivity.class);
                startActivity(intent);
            }
        });
        return mRootView;

    }

}