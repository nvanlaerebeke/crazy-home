using System;
using System.IO.Ports;

namespace PlugwiseControl;

internal class Connection {
    private readonly SerialPort _serialPort;
    private Action<string>? _onDataReceivedAction;

    public Connection(string serialPort) {
        _serialPort = new SerialPort(serialPort);
    }

    public void Open() {
        try {
            _serialPort.DataReceived += port_DataReceived;
            _serialPort.BaudRate = 115200;
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }

            _serialPort.Open();
        } catch (Exception e) {
            throw new Exception($"Could not connect to plug: {e.Message}");
        }
    }

    public void OnDataReceived(Action<string> action) {
        _onDataReceivedAction = action;
    }

    public void Send(Message.Request request) {
        var cmd = request.ToString();
        Console.WriteLine($"Sending: {cmd}");
        _serialPort.WriteLine(cmd);
    }

    private void port_DataReceived(object sender, SerialDataReceivedEventArgs e) {
        _onDataReceivedAction?.Invoke(_serialPort.ReadExisting());
    }
}
