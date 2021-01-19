import 'package:Restaurant/sign_in.dart';
import 'package:Restaurant/sign_up.dart';
import 'package:flutter/material.dart';

void main() {
  runApp(Restaurant());
}

class Restaurant extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Steak House',
      theme: ThemeData(
        primarySwatch: Colors.brown,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: HomePage(title: 'Steak House Home'),
    );
  }
}

class HomePage extends StatelessWidget {
  HomePage({Key key, this.title}) : super(key: key);

  final String title;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey,
      appBar: AppBar(
        title: Text(title),
      ),
      body: Center(
        child: Column(
          children: <Widget>[
            Padding(
              padding: EdgeInsets.all(100),
              child: Image.asset(
                "assets/SteakHouse.png",
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                left: 0,
                right: 0,
                bottom: 20,
              ),
              child: MaterialButton(
                height: 50,
                minWidth: 300,
                color: Colors.brown,
                child: Text(
                  "Sign in your account",
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
                      builder: (context) => SignIn(title: "Sign In"),
                    ),
                  );
                },
              ),
            ),
            Padding(
              padding: EdgeInsets.only(
                top: 0,
                left: 0,
                right: 0,
                bottom: 20,
              ),
              child: MaterialButton(
                height: 50,
                minWidth: 300,
                color: Colors.brown,
                child: Text(
                  "Create account",
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
                      builder: (context) => SignUp(title: "Sign Up"),
                    ),
                  );
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
