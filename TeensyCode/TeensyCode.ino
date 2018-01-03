/*
 Name:		TeensyCode.ino
 Created:	1/3/2018 6:47:21 PM
 Author:	Cynthia.MacLeod
*/

#define Debug Serial
#define Telemetry Serial4
#define ledPin 13
#define packetSize 496
#define delayMillis 15	// If you increase the delay to ~30 it stops losing bytes

const char* writeMe = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n\0";
String input;

void setup() {
	// put your setup code here, to run once:
	delay(2500);
	input = "";
	Telemetry.begin(115200);
	Debug.begin(115200);

	pinMode(ledPin, OUTPUT);
}

void loop() {
	digitalWrite(ledPin, HIGH);
	while (Telemetry.available()>0) {
		char c = Telemetry.read();
		input.concat(c);
		if (c == '\0') {
			Telemetry.print(input);
			Telemetry.print("\r\n\r\n");

			Debug.print(input);
			Debug.print("\r\n\r\n");
			input = "";

		}
	}

	char write[packetSize + 1];
	memset(write, '\0', sizeof(write));
	strncpy(write, writeMe, packetSize);
	Telemetry.print(write);
	Telemetry.print("\r\n\r\n");
	Telemetry.flush();

	delay(delayMillis);

	digitalWrite(ledPin, LOW);
	delay(delayMillis);
}