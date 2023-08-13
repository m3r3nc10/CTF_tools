import sys
import concurrent.futures
from requests.exceptions import RequestException
import requests


def dirbrute(url, wordlist):
    def check_url(word):
        word = word.strip()
        url_final = f"{url}/{word}"
        try:
            response = requests.get(url_final, timeout=5)
            if response.status_code != 404:
                print(url_final, response.status_code)
        except RequestException as error:
            print(error)

    with concurrent.futures.ThreadPoolExecutor() as executor:
        executor.map(check_url, wordlist)


if __name__ == '__main__':   
    url = sys.argv[1]
    wordlist = sys.argv[2]
    if url and wordlist:  
        with open(wordlist, "r", encoding='utf-8') as file:  
            wordlist = file.readlines()  
            dirbrute(url, wordlist) 
    else:
        print("Uso: python dirbrute.py <URL> <WORDLIST>")
