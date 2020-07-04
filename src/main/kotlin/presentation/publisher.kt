package presentation

import org.zeromq.SocketType
import org.zeromq.ZMQ
import java.lang.Thread.sleep

fun main() {
    val context = ZMQ.context(1) // New context with 1 IOThread
    val socket = context.socket(SocketType.PUB)
    socket.bind("ws://localhost:81")

    while(true) { // Send "Hello World!" every second in a different language
        socket.send("Some message")
        sleep(2000)
    }
}
