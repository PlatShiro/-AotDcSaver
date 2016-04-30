#!/usr/bin/python
from random import randint #Random Integers
import re #Regular expressions (string manipulation)
import time #Timers and sleeping threads

class myProgramUtils(object): #Program utilities object
        
        @staticmethod #The main restart method, comes here if an error occurs, takes 2 optional parameters.
        def startOrRestartProgram(ErrorOccured = False, benErrorHandler = "Null"): #Starting or restarting with error logging
                if ErrorOccured: print(benErrorHandler + "\n") #If error, print the arguments text(benErrorHandler)
                if re.compile(r'yes|y').search(str(input("Do you want to read a text file?: ")).lower()): myProgramUtils.createOrEditTextFile(str(input("Filename: "))) #If yes or no, (regex.matches)
                else: myProgramUtils().forwardToEncryptionUtils(str(input("Enter text you want to Encrypt/Decrypt: "))) #Forward message
                return None

        @staticmethod #The method to send the input into the encryption
        def forwardToEncryptionUtils(inputText = None): 
                choice = str(input("Do you want to encrypt or decrypt? (e/d): ")) #Ask user for input
                if choice != None and choice.isalpha() and re.compile(r'encrypt|e').search(choice.lower()): #If string contains e or d or encrypt or decrypt
                        if inputText != None and len(inputText) > 0: cardinalHeenanEncryptionUtils().Encrypt(inputText, True) #Send text if the string is ok
                        else: myProgramUtils().startOrRestartProgram(True, "[Log]: Text is empty.") #Return empty text error
                elif re.compile(r'decrypt|d').search(choice.lower()):
                        key = str(input("Enter your decryption key: "))
                        if key != None and len(key) > 0: #Check for empty string
                                cardinalHeenanEncryptionUtils().Decrypt(inputText, str(input("Enter your decryption key: ")), True) #Send
                        else: myProgramUtils().startOrRestartProgram(True, "[Log]: Empty Decryption key.") #Return unknown decryption key error.
                else: myProgramUtils().startOrRestartProgram(True, "[Log]: Unknown choice.") #Return unknown choice error

        @staticmethod #Editing, making and reading text files, all take place inside this method.
        def createOrEditTextFile(fileName = "Cardinal-Heenan"): #For decrypting an entire text file at once and saving it
                try: #Check if it exists
                        with open(fileName) as fileText: fileWords = fileText.read() #Read text and auto close file handler with the 'with' statement
                        print("Loaded text from: " + fileName) #Send notification
                        myProgramUtils().forwardToEncryptionUtils(fileWords) #Send the text 
                except IOError: #Catch invalid file name
                        if re.compile(r'yes|y').search(str(input("That file does not exist. Do you want to create it?: "))): #Ask them if they want to make one
                                with open(fileName, "w") as CardinalFile: CardinalFile.write(str(input("Text to save: "))) #Use with statement to automatically .close() the file.
                                myProgramUtils().startOrRestartProgram(True, "[Log]: File has been created: " + fileName) #Return with new txt file and confirmation
                        else: myProgramUtils().startOrRestartProgram(True, "[Log]: File not found: " + fileName) #Restart with FileNotFound error
                return None

        @staticmethod #For adding the encrypted/decrypted message to a text file
        def EditFile(inputString):
                if re.compile(r'yes|y').search(str(input("\nDo you want to write the Encrypted/Decrypted text to a file?: ")).lower()): #Ask them if they want to write it to a file
                        try:
                                with open(str(input("Filename: ")), 'r+') as CardinalFile: CardinalFile.write(inputString) #Ask them the filename
                                print("\nComplete. Thank you for using my program.")
                        except IOError: #if it does not exist
                                              with open("CardinalKey.txt", "w") as CardinalFile: CardinalFile.write(inputString) #make one for them
                                              print("\nThat file does not exist, but we have generated one called CardinalKey.txt for you. Thank you.") #Notify them about it
                myProgramUtils().startOrRestartProgram(False)
                
class cardinalHeenanEncryptionUtils(object): #The encryption object
        
        @staticmethod #The main method for encrypting a message, optional parameter for showing the output of the encryption.
        def Encrypt(inputString, showEncryptionLogs = False): #Encrypting the object
                operatives = { "alpha" : "qwertyuiopasdfghjklzxcvbnm", "upperalpha" : "", "nonalpha" : "1234567890!\"£$%^&*()|\\/.,;'][#=-_+}{@:><? ", "text" : "", "decryptionKey" : "" } #All normal character support
                operatives["upperalpha"] = operatives["alpha"].upper()
                for char in inputString: #Loop through all the characters
                                oldchar = char
                                listElement = "" #Use a string to save me copy and pasting 3 lines of encryption codes
                                state = ""
                                try: #Try indexing
                                        if char.isalpha(): #If alphanumeric
                                                if char.islower(): listElement = "alpha" #If lowercase
                                                else: listElement = "upperalpha" #If uppercase
                                        else: listElement = "nonalpha" #If non alphanumeric
                                        operatives["decryptionKey"] += str(operatives[listElement].index(char)) + listElement[0] #Add key location
                                        #-Replace the char with the location of it inside of the operatives list with the index value 5 indexes higher than it-#
                                        if operatives[listElement].index(char) < (len(operatives[listElement]) - 5): char = operatives[listElement][operatives[listElement].index(char) + 5]
                                        else: char = operatives[listElement][operatives[listElement].index(char) - 5] #Else replace with the index minus 5
                                        operatives["text"] += char
                                        state = "Complete." #Notify completion
                                except IndexError: #Catch errors
                                        operatives["text"] += char #Add the character from the error
                                        state = "Threw  IO Error." #Notify
                                if showEncryptionLogs: #if showing logs is enabled
                                        print("Char: " + oldchar + " ----> " + char + " ----> State: " + state) #Log
                                        time.sleep(0.2) #sleep the thread
                print("\n\nEncrypted words: " + operatives["text"] + "\nDecryption Key: " + operatives["decryptionKey"]) #Show finished product
                myProgramUtils().EditFile("Encrypted: " + operatives["text"] + "\n\nDecrypt Key: " + operatives["decryptionKey"])
        
        @staticmethod  
        def Decrypt(inputString, keyCode, showDecryptionLogs = False): #Decrypting the object
                Decryptibles = { "alpha" : "qwertyuiopasdfghjklzxcvbnm", "upperalpha" : "", "nonalpha" : "1234567890!\"£$%^&*()|\\/.,;'][#=-_+}{@:><? " } #Keys
                Decryptibles["upperalpha"] = Decryptibles["alpha"].upper()
                text = ""
                #For every char in inputstring, and use regex to get all the numbers, and then letters out of keycode, the char moves in sync with its key this way.
                for char, key, listType in zip(inputString, re.findall(r'\d+', keyCode), re.findall(r'[A-Za-z]', keyCode)): #[0-9] matches numbers, [A-Za-z] Matches alphanumeric
                        stringCharType = "" #Make new variable to pull the decryptible value out of
                        if listType == "a": stringCharType = "alpha" #If a, set to alpha
                        elif listType == "u": stringCharType = "upperalpha" #If u, set to upperalpha
                        else: stringCharType = "nonalpha" # If n, set to nonalpha
                        text += Decryptibles[stringCharType][int(key)] #add the key
                        if showDecryptionLogs: #If decyption logs are enabled
                                print("Char: " + char + " ----> " + Decryptibles[stringCharType][int(key)] + " || Key: " + key + " || ListType: " + stringCharType) #Log it
                                time.sleep(0.2) #Sleep thread
                print ("\n\nDecrypted text: " + text) #result
                myProgramUtils().EditFile("Decrypted: " + text)
        
                
#<Program Start with myProgramUtils Instance>
myProgramUtils().startOrRestartProgram()
