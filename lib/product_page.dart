import 'dart:convert';

import 'package:Restaurant/api_urls.dart';
import 'package:Restaurant/cart.dart';
import 'package:Restaurant/cart_page.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class ProductPage extends StatefulWidget {
  ProductPage({this.title});

  final String title;

  @override
  _ProductPageState createState() => _ProductPageState();
}

class _ProductPageState extends State<ProductPage> {
  @override
  void initState() {
    fetchData();
    super.initState();
  }

  dynamic _product;
  List<dynamic> _productsInMenu = List();
  String _quantity = "";
  String _price = "";
  List<dynamic> _allergens = new List();
  int _pieces = 1;
  // dynamic _base64;

  Future fetchData() async {
    var url = ApiUrl.getProductDetails + widget.title;
    var result = await http.get(url);
    if (result.statusCode == 200) {
      setState(() {
        _product = json.decode(result.body);
        _quantity = _product['Quantity'].toString() + " g";
        _price = _product['Price'].toString() + " RON";
        _allergens = _product['Allergens'];
        if (_product['ProductType'] == 1) {
          _productsInMenu = _product['Products'];
        }
        // _base64 = base64Encode(_product['Image1']);
      });
    }
  }

  Future<void> showAllertDialog() async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: Colors.grey,
          title: Text(
            "Product added to cart.\t\nSee cart?",
          ),
          actions: <Widget>[
            MaterialButton(
              height: 50,
              minWidth: 100,
              color: Colors.brown,
              child: Text(
                "Yes",
                style: TextStyle(
                  fontFamily: "OpenSans",
                  fontSize: 20,
                  color: Colors.white,
                ),
              ),
              onPressed: () {
                Navigator.pop(context);
                Navigator.pop(context);
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => CartPage(title: "Cart"),
                  ),
                );
              },
            ),
            MaterialButton(
              height: 50,
              minWidth: 100,
              color: Colors.brown,
              child: Text(
                "No",
                style: TextStyle(
                  fontFamily: "OpenSans",
                  fontSize: 20,
                  color: Colors.white,
                ),
              ),
              onPressed: () {
                Navigator.of(context).pop();
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey,
      appBar: AppBar(
        title: Padding(
          padding: EdgeInsets.only(
            left: MediaQuery.of(context).size.width / 6,
          ),
          child: Text(widget.title),
        ),
      ),
      body: SingleChildScrollView(
        child: _product == null
            ? Center(
                child: Padding(
                  padding: EdgeInsets.only(top: 100),
                  child: Text(
                    "Loading...",
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                    textAlign: TextAlign.center,
                  ),
                ),
              )
            : Center(
                child: Padding(
                  padding: EdgeInsets.only(top: 50),
                  child: Column(
                    children: <Widget>[
                      ListTile(
                        title: Text(
                          "Quantity: " + _quantity,
                          style: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                          ),
                          textAlign: TextAlign.center,
                        ),
                      ),
                      ListTile(
                        title: Text(
                          "Price: " + _price,
                          style: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                          ),
                          textAlign: TextAlign.center,
                        ),
                      ),
                      Padding(
                        padding: EdgeInsets.only(
                          left: MediaQuery.of(context).size.width / 3,
                          right: MediaQuery.of(context).size.width / 3,
                        ),
                        child: DataTable(
                          headingTextStyle: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                            color: Colors.black,
                          ),
                          dataTextStyle: TextStyle(
                            fontSize: 20,
                            color: Colors.black,
                          ),
                          columns: [
                            DataColumn(
                              label: Text("Allergens"),
                            ),
                          ],
                          rows: List<DataRow>.generate(
                            _allergens.length == 0 ? 1 : _allergens.length,
                            (index) => DataRow(
                              cells: [
                                DataCell(
                                  _allergens.length == 0
                                      ? Text(
                                          "This product has no allergens",
                                          style: TextStyle(
                                            fontSize: 13,
                                          ),
                                        )
                                      : Text(_allergens[index]),
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),
                      _productsInMenu.length != 0
                          ? Padding(
                              padding: EdgeInsets.only(
                                left: MediaQuery.of(context).size.width / 6,
                                right: MediaQuery.of(context).size.width / 6,
                              ),
                              child: DataTable(
                                headingTextStyle: TextStyle(
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold,
                                  color: Colors.black,
                                ),
                                dataTextStyle: TextStyle(
                                  fontSize: 20,
                                  color: Colors.black,
                                ),
                                columns: [
                                  DataColumn(
                                    label: Text("Products"),
                                  ),
                                ],
                                rows: List<DataRow>.generate(
                                  _allergens.length,
                                  (index) => DataRow(
                                    cells: [
                                      DataCell(
                                        Text(_productsInMenu[index]['Name']),
                                      ),
                                    ],
                                  ),
                                ),
                              ),
                            )
                          : Text(""),
                      Row(
                        children: [
                          Padding(
                            padding: EdgeInsets.only(
                              left: 100,
                              right: 20,
                            ),
                            child: FloatingActionButton(
                              child: Text(
                                "-",
                                style: TextStyle(
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                              onPressed: () {
                                setState(() {
                                  if (_pieces > 1) {
                                    _pieces -= 1;
                                  }
                                });
                              },
                            ),
                          ),
                          Expanded(
                            child: Padding(
                              padding: EdgeInsets.only(
                                left: 0,
                                right: 0,
                              ),
                              child: TextField(
                                enabled: false,
                                decoration: InputDecoration(
                                  floatingLabelBehavior:
                                      FloatingLabelBehavior.never,
                                  border: OutlineInputBorder(
                                    borderSide: BorderSide(color: Colors.black),
                                  ),
                                  labelText: _pieces.toString(),
                                ),
                              ),
                            ),
                          ),
                          Padding(
                            padding: EdgeInsets.only(
                              left: 20,
                              right: 100,
                            ),
                            child: FloatingActionButton(
                              child: Text(
                                "+",
                                style: TextStyle(
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                              onPressed: () {
                                setState(() {
                                  if (_pieces < 20) {
                                    _pieces += 1;
                                  }
                                });
                              },
                            ),
                          ),
                        ],
                      ),
                      Padding(
                        padding: EdgeInsets.only(
                          top: 50,
                          left: 50,
                          right: 50,
                          bottom: 50,
                        ),
                        child: MaterialButton(
                          height: 50,
                          minWidth: 300,
                          color: Colors.brown,
                          child: Text(
                            "Add to cart",
                            style: TextStyle(
                              fontFamily: "OpenSans",
                              fontSize: 20,
                              color: Colors.white,
                            ),
                          ),
                          onPressed: () {
                            _product['Pieces'] = _pieces;
                            Cart.products.add(_product);
                            showAllertDialog();
                          },
                        ),
                      ),
                    ],
                  ),
                ),
              ),
      ),
    );
  }
}
