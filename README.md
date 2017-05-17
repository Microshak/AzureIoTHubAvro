# Azure IoT Hub Avro (Proof of concept)
Example of Moving Data From Device to Azure IotHub using Deflated Avro

## Components
IoT Hub
Stream Analytics
Computer for simulator

## Set up
Iot Hub
  Get your IoT hub up and running
  Create a device manually
  Put the key and URI in the simulator

Stream Analytics
  Create a ASA Function and put in the ASAFunction.JS content
  Create your input and use the ASA.SQL file for input
  
 # Explination
 What we are doing in using deflated Avro. Avro will not send compex objects.  
 So we are passing in the objects as a JSON string and parsing them on the other side with a JavaScript function.
 
