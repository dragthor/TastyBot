# TastyTrade Automated Bot w/Unofficial API

Cross platform (Windows, Linux, and macOS) automated bot for TastyTrade customers.

Plug-in your own rules.  Here is an example of what I want the TastyBot to do:

- Do I already have a SPY position open?
- Do I have sufficient buying power?
- Is the VIX above 20?
- Did SPY drop 2%?
- All are true? Then sell a 45 DTE $5 wide SPY put spread at the 10 delta

Referral link - https://start.tastytrade.com/#/login?referralCode=SP8DSHF682

## Warning

The code will actually place an order when `_liveOrdersEnabled` is true (default is false).

## Safety and Security

Never give your password to anyone.  This code is open source.  You can verify that the TastyBot does not save nor transmit your password.  All communication between TastyTrade and the TastyBot are encrypted using SSL.

## Disclaimer

This is an unofficial, reverse-engineered API for TastyTrade.  Zero inside information and knowledge was used to create it.  There is no implied warranty for any actions and results which arise from using it.  Any examples, rules, or algorithms are intended for educational purposes only and not financial advice.
