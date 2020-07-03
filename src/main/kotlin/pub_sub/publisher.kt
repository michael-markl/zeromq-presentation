package pub_sub

import org.zeromq.SocketType
import org.zeromq.ZMQ
import java.lang.Thread.sleep

fun main() {
    val context = ZMQ.context(1) // New context with 1 IOThread
    val socket = context.socket(SocketType.PUB)
    socket.bind("tcp://localhost:2000")

    val messages = mapOf(
        "de" to "Hallo Welt!",
        "en" to "Hello World!",
        "fr" to "Salut le monde!"
    )

    while(true) { // Send "Hello World!" every second in a different language
        for ((key, message) in messages) {
            socket.send("$key $message")
            println("Just pinged the world in the language: $key.")
            sleep(1000)
        }
    }
}
