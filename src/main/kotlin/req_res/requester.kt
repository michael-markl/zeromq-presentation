package req_res

import org.zeromq.SocketType
import org.zeromq.ZMQ

fun main() {
    val context = ZMQ.context(1)
    val socket = context.socket(SocketType.REQ)
    socket.connect("tcp://localhost:2000")

    val greetings = arrayOf("Hello", "Salut", "Hallo")

    for (greeting in greetings) {
        socket.send(greeting)
        val reply = socket.recvStr()
        println("Sent '$greeting', received '$reply'")
    }
}
