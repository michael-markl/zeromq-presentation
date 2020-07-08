package req_res

import org.zeromq.SocketType
import org.zeromq.ZMQ

fun main() {
    val context = ZMQ.context(1) // New context with 1 IOThread
    val socket = context.socket(SocketType.REP)
    socket.bind("tcp://localhost:2000")

    val messages = mapOf(
        "Hello" to "World!",
        "Hallo" to "Welt!",
        "Salut" to "tout le monde!"
    )

    while(true) {
        val request = socket.recvStr()
        val response = messages[request]
        if (response != null) socket.send(response)
        else socket.send("Invalid request.")
    }
}
