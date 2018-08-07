package com.odms.mahtab.dms.Fragment;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.odms.mahtab.dms.R;


public class Fragment_OrderForthStep extends Fragment {

    public Fragment_OrderForthStep() {
        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }
    private View mRootView;
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,Bundle savedInstanceState) {
        if(mRootView==null){
            mRootView = inflater.inflate(R.layout.fragment_orderfourthstep, container, false);
            //......
        }
        return mRootView;

    }

}