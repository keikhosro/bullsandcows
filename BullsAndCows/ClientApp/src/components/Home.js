import React, { Component } from 'react';
import { Terminal } from 'xterm';
import 'xterm/css/xterm.css';
import * as signalR from '@microsoft/signalr';

export class Home extends Component {
  async componentDidMount() {
    const prompt = function (term) {
      term.write('\r\n$ ');
    };

    await this.getGameObject();

    this.term = new Terminal();
    this.term.open(this.terminalDiv);
    this.term.write("Welcome to a game of Bulls & Cows!");

    this.connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

    this.connection.on("ReceiveMessage", (gameObj, message) => {
      this.setState({ gameObj });
      if (message) {
        prompt(this.term);
        this.term.write(message);
      }
    });

    this.connection.start()
      .then(() => this.connection.invoke("GetGuess", this.state.gameObj))
      .catch(function (err) {
        return console.error(err.toString());
      });

    var input = "";
    this.term.onKey(e => {
      const printable = !e.domEvent.altKey && !e.domEvent.altGraphKey && !e.domEvent.ctrlKey && !e.domEvent.metaKey;
      if (e.domEvent.keyCode === 13) {
        this.connection.invoke("SendAnswer", this.state.gameObj, input)
          .then(() => this.connection.invoke("GetGuess", this.state.gameObj).catch(function (err) {
            return console.error(err.toString());
          }))
          .catch(function (err) {
            return console.error(err.toString());
          });
        input = "";
      } else if (e.domEvent.keyCode === 8) {
        // Do not delete the prompt
        if (this.term._core.buffer.x > 2) {
          this.term.write('\b \b');
        }
      } else if (printable) {
        input += e.key;
        this.term.write(e.key);
      }
    });
  }

  render() {
    return (
      <div id="terminal" ref={(ref) => this.terminalDiv = ref}></div>
    );
  }

  async getGameObject() {
    const response = await fetch('game');
    const data = await response.json();
    this.setState({ gameObj: data, loading: false });
  }
}
