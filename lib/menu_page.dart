import 'dart:convert';

import 'package:Restaurant/api_urls.dart';
import 'package:Restaurant/cart_page.dart';
import 'package:Restaurant/product_page.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class MenuPage extends StatefulWidget {
  MenuPage({this.title});

  final String title;

  @override
  _MenuPageState createState() => _MenuPageState();
}

class _MenuPageState extends State<MenuPage> {
  Map<String, List<dynamic>> _productsStore = new Map();

  List<Widget> _drawersWidgets = new List();
  List<dynamic> _categories = new List();
  List<dynamic> _products = new List();
  String _category = "Loading...";

  @override
  void initState() {
    fetchCategories();
    fetchProducts("Appetizers          ");
    super.initState();
  }

  Future fetchCategories() async {
    var url = ApiUrl.getCategories;
    var result = await http.get(url);
    if (result.statusCode == 200) {
      setState(() {
        _categories = json.decode(result.body);
      });
    }
  }

  Future fetchProducts(String category) async {
    if (_productsStore[category] == null) {
      var url = ApiUrl.getProductsByCategory + category;
      var result = await http.get(url);
      if (result.statusCode == 200) {
        setState(() {
          _category = category;
          _products = json.decode(result.body);
          _productsStore[category] = _products;
        });
      }
    } else {
      setState(() {
        _category = category;
        _products = _productsStore[category];
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    _drawersWidgets = new List();
    _drawersWidgets.add(DrawerHeader(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: <Widget>[
          // MaterialButton(
          //   child: Text(
          //     "See orders",
          //     style: TextStyle(
          //       fontFamily: "OpenSans",
          //       fontSize: 30,
          //       color: Colors.white,
          //       fontWeight: FontWeight.bold,
          //     ),
          //   ),
          //   onPressed: () {
          //     Navigator.pop(context);
          //     Navigator.pop(context);
          //   },
          // ),
          MaterialButton(
            child: Text(
              "See cart",
              style: TextStyle(
                fontFamily: "OpenSans",
                fontSize: 30,
                color: Colors.white,
                fontWeight: FontWeight.bold,
              ),
            ),
            onPressed: () {
              Navigator.pop(context);
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => CartPage(
                    title: "Cart",
                  ),
                ),
              );
            },
          ),
          Padding(
            padding: EdgeInsets.only(top: 5),
            child: Text(
              'Category',
              style: TextStyle(
                color: Colors.white,
                fontSize: 30,
              ),
            ),
          ),
        ],
      ),
      decoration: BoxDecoration(
        color: Colors.brown,
      ),
    ));
    for (var category in _categories) {
      _drawersWidgets.add(
        ListTile(
          title: Text(
            category,
            style: TextStyle(
              color: Colors.brown,
              fontSize: 20,
            ),
          ),
          onTap: () {
            if (category == _category) {
              Navigator.pop(context);
            } else {
              setState(() {
                _products = new List();
                _category = "Loading...";
                fetchProducts(category);
              });
              Navigator.pop(context);
            }
          },
        ),
      );
    }

    return Scaffold(
      backgroundColor: Colors.grey,
      appBar: AppBar(
        title: Text(widget.title),
      ),
      drawer: Drawer(
        child: ListView(
          padding: EdgeInsets.zero,
          children: _drawersWidgets,
        ),
      ),
      body: SingleChildScrollView(
        child: Center(
          child: DataTable(
            dataRowHeight: 80,
            headingRowHeight: 100,
            columns: [
              DataColumn(
                label: Padding(
                  padding:
                      EdgeInsets.only(left: MediaQuery.of(context).size.width) /
                          6,
                  child: Text(
                    _category,
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      fontWeight: FontWeight.bold,
                      fontSize: 30,
                    ),
                  ),
                ),
              ),
            ],
            rows: List<DataRow>.generate(
              _products.length,
              (index) => DataRow(
                cells: <DataCell>[
                  DataCell(
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[
                        Padding(
                          padding: EdgeInsets.only(top: 10),
                          child: Text(
                            _products[index]['Name'],
                            textAlign: TextAlign.left,
                            style: TextStyle(
                              fontWeight: FontWeight.bold,
                              fontSize: 20,
                            ),
                          ),
                        ),
                        Padding(
                          padding: EdgeInsets.only(top: 15),
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: <Widget>[
                              Text(
                                _products[index]['Quantity'].toString() + " g",
                                textAlign: TextAlign.left,
                                style: TextStyle(
                                  fontSize: 15,
                                ),
                              ),
                              Padding(
                                padding: EdgeInsets.only(
                                  left: MediaQuery.of(context).size.width / 2,
                                ),
                                child: Text(
                                  _products[index]['Price'].toString() + " RON",
                                  textAlign: TextAlign.right,
                                  style: TextStyle(
                                    fontSize: 15,
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),
                    onTap: () {
                      var name = _products[index]['Name'];
                      Navigator.push(
                        context,
                        MaterialPageRoute(
                          builder: (context) => ProductPage(
                            title: name,
                          ),
                        ),
                      );
                    },
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
