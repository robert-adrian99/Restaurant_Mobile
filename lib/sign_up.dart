import 'dart:convert';

import 'package:Restaurant/api_urls.dart';
import 'package:Restaurant/cart.dart';
import 'package:Restaurant/menu_page.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class SignUp extends StatefulWidget {
  SignUp({this.title});

  final String title;

  @override
  _SignUpState createState() => _SignUpState();
}

class _SignUpState extends State<SignUp> {
  final GlobalKey<ScaffoldState> _scaffoldKey = new GlobalKey<ScaffoldState>();

  String _firstName = "";
  String _lastName = "";
  String _email = "";
  String _phoneNumber = "";
  String _deliveryAddress = "";
  String _password = "";
  String _confirmPassword = "";

  showErrorSnackBar() {
    _scaffoldKey.currentState.showSnackBar(
      new SnackBar(
        content: Text(
          "There is an error!",
          style: TextStyle(
              fontFamily: 'OpenSans',
              fontWeight: FontWeight.bold,
              fontSize: 20),
          textAlign: TextAlign.center,
        ),
        backgroundColor: Colors.red,
      ),
    );
  }

  Future postData() async {
    var body = jsonEncode({
      "FirstName": _firstName,
      "LastName": _lastName,
      "Email": _email,
      "Phone": _phoneNumber,
      "Address": _deliveryAddress,
      "Password": _password
    });

    var result = await http.post(
      ApiUrl.addUser,
      body: body,
      headers: {
        "content-type": "application/json",
        "accept": "application/json",
      },
    );
    if (result.statusCode == 200) {
      Cart.products = new List();
      Navigator.pop(context);
      Navigator.push(
        context,
        MaterialPageRoute(
          builder: (context) => MenuPage(
            title: "Menu",
          ),
        ),
      );
    } else {
      showErrorSnackBar();
    }
  }

  Future<void> showAllertDialog(String allertMessage) async {
    return showDialog<void>(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: Colors.grey,
          title: Text(
            allertMessage,
          ),
          actions: <Widget>[
            MaterialButton(
              height: 50,
              minWidth: 100,
              color: Colors.brown,
              child: Text(
                "OK",
                style: TextStyle(
                  fontFamily: "OpenSans",
                  fontSize: 20,
                  color: Colors.white,
                ),
              ),
              onPressed: () {
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
      key: _scaffoldKey,
      backgroundColor: Colors.grey,
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: SingleChildScrollView(
        child: Column(
          children: <Widget>[
            Padding(
              padding: EdgeInsets.only(
                top: 50,
                bottom: 20,
                left: 50,
                right: 50,
              ),
              child: TextField(
                decoration: InputDecoration(
                  floatingLabelBehavior: FloatingLabelBehavior.auto,
                  labelText: "First Name*",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.black,
                    ),
                  ),
                ),
                onChanged: (value) {
                  setState(() {
                    _firstName = value;
                  });
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                bottom: 20,
                left: 50,
                right: 50,
              ),
              child: TextField(
                decoration: InputDecoration(
                  floatingLabelBehavior: FloatingLabelBehavior.auto,
                  labelText: "Last Name*",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.black,
                    ),
                  ),
                ),
                onChanged: (value) {
                  setState(() {
                    _lastName = value;
                  });
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                bottom: 20,
                left: 50,
                right: 50,
              ),
              child: TextField(
                decoration: InputDecoration(
                  floatingLabelBehavior: FloatingLabelBehavior.auto,
                  labelText: "Email*",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.white,
                    ),
                  ),
                ),
                onChanged: (value) {
                  setState(() {
                    _email = value;
                  });
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                bottom: 20,
                left: 50,
                right: 50,
              ),
              child: TextField(
                decoration: InputDecoration(
                  floatingLabelBehavior: FloatingLabelBehavior.auto,
                  labelText: "Phone Number*",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.white,
                    ),
                  ),
                ),
                onChanged: (value) {
                  setState(() {
                    _phoneNumber = value;
                  });
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                bottom: 20,
                left: 50,
                right: 50,
              ),
              child: TextField(
                decoration: InputDecoration(
                  floatingLabelBehavior: FloatingLabelBehavior.auto,
                  labelText: "Delivery Address*",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.white,
                    ),
                  ),
                ),
                onChanged: (value) {
                  setState(() {
                    _deliveryAddress = value;
                  });
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                bottom: 30,
                left: 50,
                right: 50,
              ),
              child: TextField(
                obscureText: true,
                decoration: InputDecoration(
                  floatingLabelBehavior: FloatingLabelBehavior.auto,
                  labelText: "Password*",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.white,
                    ),
                  ),
                ),
                onChanged: (value) {
                  setState(() {
                    _password = value;
                  });
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                bottom: 30,
                left: 50,
                right: 50,
              ),
              child: TextField(
                obscureText: true,
                decoration: InputDecoration(
                  floatingLabelBehavior: FloatingLabelBehavior.auto,
                  labelText: "Confirm Password*",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.white,
                    ),
                  ),
                ),
                onChanged: (value) {
                  setState(() {
                    _confirmPassword = value;
                  });
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(bottom: 50),
              child: MaterialButton(
                height: 50,
                minWidth: 300,
                color: Colors.brown,
                child: Text(
                  "SIGN UP",
                  style: TextStyle(
                    fontFamily: "OpenSans",
                    fontSize: 20,
                    color: Colors.white,
                  ),
                ),
                onPressed: () {
                  if (_firstName == "" ||
                      _lastName == "" ||
                      _email == "" ||
                      _phoneNumber == "" ||
                      _deliveryAddress == "" ||
                      _password == "" ||
                      _confirmPassword == "") {
                    showAllertDialog('Fields marked with * are mandatory!');
                  } else if (_password == _confirmPassword) {
                    if (RegExp(r"@steakhouse.com$").hasMatch(_email) ||
                        RegExp(r"@steak-house.com$").hasMatch(_email)) {
                      showAllertDialog(
                          "There is an error with the email domain!");
                    } else {
                      RegExp firstNameRegex = RegExp(r"^[A-Z]{1}[a-z]+");
                      RegExp lastNameRegex = RegExp(r"^[A-Z]{1}[a-z]+");
                      RegExp emailRegex = RegExp(
                          r"^[A-Za-z0-9._]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");
                      RegExp phoneRegex = RegExp(r"^0[0-9]{9}");
                      RegExp deliveryRegex = RegExp(r"[A-Za-z\s]+[0-9]+");
                      if (!firstNameRegex.hasMatch(_firstName) ||
                          !lastNameRegex.hasMatch(_lastName) ||
                          !emailRegex.hasMatch(_email) ||
                          !phoneRegex.hasMatch(_phoneNumber) ||
                          !deliveryRegex.hasMatch(_deliveryAddress) ||
                          _password.length < 8) {
                        showAllertDialog(
                            "Some fields are not corresponding the formats");
                      } else {
                        postData();
                      }
                    }
                  } else {
                    showAllertDialog("Password is not the same!");
                  }
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
