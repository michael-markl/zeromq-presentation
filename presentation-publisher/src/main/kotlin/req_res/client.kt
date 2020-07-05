package presentation.req_res

import org.zeromq.SocketType
import org.zeromq.ZMQ
import kotlin.system.measureTimeMillis

fun main() {
    val context = ZMQ.context(1)

    val socket = context.socket(SocketType.REQ)
    socket.connect("tcp://localhost:2000")

    socket.send("Salut")
    println("Server responded with \"${socket.recvStr()}\" on the request \"Salut\".")

    val requests = 5
    val millis = measureTimeMillis {
        for (i in 1..requests) {
            socket.send("Hello")
            val reply = socket.recvStr()
            if (reply != "World") throw Error()
        }
    }
    println("Received $requests 'World'-replies in $millis ms.")
}
