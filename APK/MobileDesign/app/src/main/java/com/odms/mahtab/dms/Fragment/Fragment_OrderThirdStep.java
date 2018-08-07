package com.odms.mahtab.dms.Fragment;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.odms.mahtab.dms.Controller.OrderActivity;
import com.odms.mahtab.dms.R;


/**
 * Created by Anu on 22/04/17.
 */



public class Fragment_OrderThirdStep extends Fragment {

    private View mRootView;
    public Fragment_OrderThirdStep() {
        // Required empty public constructor
    }
    public static Fragment_OrderThirdStep newInstance() {

            Fragment_OrderThirdStep fragment = new Fragment_OrderThirdStep();

        return fragment;
    }
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,Bundle savedInstanceState) {


        if(mRootView==null){
            mRootView = inflater.inflate(R.layout.fragment_orderthirdstep, container, false);
        }
        return mRootView;

    }


}