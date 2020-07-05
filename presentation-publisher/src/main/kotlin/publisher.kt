package presentation

import org.zeromq.SocketType
import org.zeromq.ZMQ

fun main() {
    val context = ZMQ.context(1) // New context with 1 IOThread
    val publisher = context.socket(SocketType.PUSH)
    publisher.connect("tcp://*:3000")
    var step = 0.toByte()
    while(true) {
        print("Slide $step. Press enter for the next or anything else for the previous.");
        val input = readLine()
        if (input.isNullOrEmpty()) {
            publisher.send(ByteArray(1) { ++step })
        } else {
            publisher.send(ByteArray(1) { --step })
        }
    }
}
