#include <SoftwareSerial.h>

const int TX_BT = 11; // Yellow
const int RX_BT = 10; // Bluefs
SoftwareSerial btSerial(TX_BT, RX_BT);

const int LED_PIN = 7;
const int RELAY_PIN = 6;

void setup() {
  // put your setup code here, to run once:
  pinMode(LED_PIN, OUTPUT);
  pinMode(RELAY_PIN, OUTPUT);
  Serial.begin(9600);
  Serial.println("USB Connected Blue 10 rx");
  btSerial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:

  if(btSerial.available()) 
      {      
        int commandSize = (int)btSerial.read();    

        char command[commandSize];      
        int commandPos = 0;      
        while(commandPos < commandSize) 
        {        
          if(btSerial.available()) 
          {          
            command[commandPos] = (char)btSerial.read();          
            commandPos++;        
            }      
          }      
        
        command[commandPos] = 0;      
        processCommand(command);  
      }
}

void processCommand(char* command) 
{
  Serial.println(command);
  String strCommand = (String)command;
  if(strCommand == "on")
  {
    Serial.println(1);
    digitalWrite(LED_PIN, HIGH);
    digitalWrite(RELAY_PIN, HIGH);
  }
  else if(strCommand == "off")
  {
    Serial.println(0);
    digitalWrite(LED_PIN, LOW);
    digitalWrite(RELAY_PIN, LOW);
  }
}
