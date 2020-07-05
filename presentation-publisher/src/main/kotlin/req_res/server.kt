package presentation.req_res

import org.zeromq.SocketType
import org.zeromq.ZMQ

fun main() {
    val context = ZMQ.context(1) // New context with 1 IOThread
    val socket = context.socket(SocketType.REP)
    socket.bind("tcp://localhost:2000")
    while(true) {
        val request = socket.recvStr()
        if (request == "Hello") socket.send("World")
        else socket.send("Invalid request.")
    }
}
