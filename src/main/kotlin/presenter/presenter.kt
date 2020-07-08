@file:JvmName("Main")

package presenter

import org.zeromq.SocketType
import org.zeromq.ZMQ

fun main() {
    val context = ZMQ.context(1) // New context with 1 IOThread
    val push = context.socket(SocketType.PUB)
    push.connect("tcp://*:3000")
    Thread.sleep(100) // Give option to connect before sending first message

    var step = 0.toByte()
    while(true) {
        push.send(ByteArray(1) { step })
        print("Slide $step. Press enter for the next or anything else for the previous.");
        if (readLine().isNullOrEmpty()) step++  else step--
    }
}
