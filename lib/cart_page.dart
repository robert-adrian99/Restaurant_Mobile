import 'dart:convert';

import 'package:Restaurant/api_urls.dart';
import 'package:Restaurant/cart.dart';
import 'package:Restaurant/menu_page.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class CartPage extends StatefulWidget {
  CartPage({this.title});

  final String title;

  @override
  _CartPageState createState() => _CartPageState();
}

class _CartPageState extends State<CartPage> {
  double _price = 0;

  Future<void> showAllertDialog(String message) async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: Colors.grey,
          title: Text(
            message,
          ),
          actions: <Widget>[
            MaterialButton(
              height: 50,
              minWidth: 100,
              color: Colors.brown,
              child: Text(
                "Go to Menu",
                style: TextStyle(
                  fontFamily: "OpenSans",
                  fontSize: 20,
                  color: Colors.white,
                ),
              ),
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => MenuPage(title: "Menu"),
                  ),
                );
              },
            ),
          ],
        );
      },
    );
  }

  Future postData() async {
    var body = jsonEncode({
      "Price": _price,
      "Products": Cart.products,
    });

    var result = await http.post(
      ApiUrl.addOrder,
      body: body,
      headers: {
        "content-type": "application/json",
        "accept": "application/json",
      },
    );
    Cart.products = new List();
    showAllertDialog("Order placed!");
  }

  @override
  Widget build(BuildContext context) {
    setState(() {
      for (var product in Cart.products) {
        _price += product['Price'] * product['Pieces'];
      }
    });

    return Scaffold(
      backgroundColor: Colors.grey,
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: SingleChildScrollView(
        child: Column(
          children: <Widget>[
            Padding(
              padding: EdgeInsets.only(top: 20),
              child: DataTable(
                dataRowHeight: 80,
                columns: [
                  DataColumn(
                    label: Text(
                      "This is your cart:",
                      style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ],
                rows: List<DataRow>.generate(
                  Cart.products.length,
                  (index) => DataRow(
                    cells: <DataCell>[
                      DataCell(
                        Column(
                          children: <Widget>[
                            Padding(
                              padding: EdgeInsets.only(top: 10),
                              child: Text(
                                Cart.products[index]['Name'],
                                style: TextStyle(fontSize: 18),
                              ),
                            ),
                            Row(
                              children: <Widget>[
                                Padding(
                                  padding: EdgeInsets.only(
                                    top: 15,
                                    left: 50,
                                  ),
                                  child: Text(
                                    Cart.products[index]['Price'].toString() +
                                        " RON",
                                    style: TextStyle(fontSize: 18),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.only(
                                    top: 15,
                                    left: 100,
                                  ),
                                  child: Text(
                                    "x " +
                                        Cart.products[index]['Pieces']
                                            .toString(),
                                    style: TextStyle(fontSize: 18),
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 50,
                left: 50,
                right: 50,
              ),
              child: Text(
                "Total: " + _price.toString(),
                style: TextStyle(
                  fontWeight: FontWeight.bold,
                  fontSize: 20,
                ),
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 50,
                left: 50,
                right: 50,
              ),
              child: MaterialButton(
                height: 50,
                minWidth: 300,
                color: Colors.brown,
                child: Text(
                  "Place order",
                  style: TextStyle(
                    fontFamily: "OpenSans",
                    fontSize: 20,
                    color: Colors.white,
                  ),
                ),
                onPressed: () {
                  postData();
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
