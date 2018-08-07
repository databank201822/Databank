package com.odms.mahtab.dms.Model;

public class M_OrderSku {

    private int _id,_outletId,_outletCode,_routeId,_orderStatus;
    private String _outletName;
    private String _orderCS;

    public M_OrderSku(){

    }

    public M_OrderSku(int id, int outletId, int outletCode, String outletName, int routeId, String orderCS, int orderStatus ) {
        this._id = id;
        this._outletId = outletId;
        this._outletCode = outletCode;
        this._routeId = routeId;
        this._orderStatus = orderStatus;
        this._outletName = outletName;
        this._orderCS = orderCS;
    }
    public M_OrderSku(int outletId, int outletCode, String outletName, int routeId, String orderCS, int orderStatus ) {

        this._outletId = outletId;
        this._outletCode = outletCode;
        this._routeId = routeId;
        this._orderStatus = orderStatus;
        this._outletName = outletName;
        this._orderCS = orderCS;
    }

    public int get_id() {
        return _id;
    }

    public void set_id(int _id) {
        this._id = _id;
    }

    public int get_outletId() {
        return _outletId;
    }

    public void set_outletId(int outletId) {
        this._outletId = outletId;
    }

    public int get_outletCode() {
        return _outletCode;
    }

    public void set_outletCode(int outletCode) {
        this._outletCode = outletCode;
    }

    public int get_routeId() {
        return _routeId;
    }

    public void set_routeId(int routeId) {
        this._routeId = routeId;
    }

    public int get_orderStatus() {
        return _orderStatus;
    }

    public void set_orderStatus(int orderStatus) {
        this._orderStatus = orderStatus;
    }

    public String get_outletName() {
        return _outletName;
    }

    public void set_outletName(String outletName) {
        this._outletName = outletName;
    }

    public String get_orderCS() {
        return _orderCS;
    }

    public void set_OrderCS(String orderCS) {
        this._orderCS = orderCS;
    }



}
