package presentation

import org.zeromq.SocketType
import org.zeromq.ZMQ

fun main() {
    val context = ZMQ.context(1)
    val socket = context.socket(SocketType.SUB)
    socket.connect("tcp://localhost:2000")
    socket.subscribe("de")
    socket.subscribe("en")

    while(true) {
        val message = socket.recvStr().substringAfter(" ")
        println("Received the message: \"${message}\"")
    }
}
