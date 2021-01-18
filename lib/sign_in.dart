import 'package:Restaurant/cart.dart';
import 'package:Restaurant/menu_page.dart';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';

import 'package:Restaurant/api_urls.dart';

class SignIn extends StatefulWidget {
  SignIn({this.title});

  final String title;

  @override
  _SignInState createState() => _SignInState();
}

class _SignInState extends State<SignIn> {
  final GlobalKey<ScaffoldState> _scaffoldKey = new GlobalKey<ScaffoldState>();
  String _statusLabel = "";
  Color _statusColor = Colors.red;
  String _email = "";
  String _password = "";

  showErrorSnackBar() {
    _scaffoldKey.currentState.showSnackBar(
      new SnackBar(
        content: Text(
          "Incorrect email or password!",
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

  Future fetchData() async {
    var url = ApiUrl.getUserByEmailAndPassword +
        'email=' +
        _email +
        '&password=' +
        _password;
    var result = await http.get(url);
    if (result.statusCode == 200) {
      setState(() {
        _statusLabel = "";
      });
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
      setState(() {
        _statusLabel = "";
      });
      showErrorSnackBar();
    }
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
                bottom: 50,
                left: 130,
                right: 130,
                top: 50,
              ),
              child: Image.asset("assets/SteakHouse.png"),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                bottom: 20,
                left: 50,
                right: 50,
              ),
              child: Text(
                _statusLabel,
                style: TextStyle(color: _statusColor, fontSize: 20),
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
                  labelText: "Email",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.black,
                    ),
                  ),
                ),
                onChanged: (value) {
                  _email = value;
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                bottom: 50,
                left: 50,
                right: 50,
              ),
              child: TextField(
                obscureText: true,
                decoration: InputDecoration(
                  floatingLabelBehavior: FloatingLabelBehavior.auto,
                  labelText: "Password",
                  border: OutlineInputBorder(
                    borderSide: BorderSide(
                      color: Colors.black,
                    ),
                  ),
                ),
                onChanged: (value) {
                  _password = value;
                },
              ),
            ),
            MaterialButton(
              height: 50,
              minWidth: 300,
              color: Colors.brown,
              child: Text(
                "Sign in",
                style: TextStyle(
                  fontFamily: "OpenSans",
                  fontSize: 20,
                  color: Colors.white,
                ),
              ),
              onPressed: () {
                if (RegExp(r"^[A-Za-z0-9._]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
                    .hasMatch(_email)) {
                  setState(() {
                    _statusColor = Colors.brown;
                    _statusLabel = "Signing in...";
                  });
                  fetchData();
                } else {
                  setState(() {
                    _statusColor = Colors.red;
                    _statusLabel = "Invalid email format";
                  });
                }
              },
            )
          ],
        ),
      ),
    );
  }
}
