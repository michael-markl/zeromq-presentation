#!/bin/sh

RC=1
while [ $RC -ne 0 ]; do
   clear
   /usr/bin/mono ./bin/Debug/PresentationDealer.exe
   RC=$?
done
