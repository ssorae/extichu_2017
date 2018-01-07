#include <funapi.h>

#include <string>

#ifndef EXTICHU_PLAYER_H_
#define EXTICHU_PLAYER_H_

class ExtPlayer {
public:
    ExtPlayer(SessionId _id, std::string _nickname) 
    : id(_id), nickname(_nickname) {};

private:
    SessionId id;
    std::string nickname;

public:
    SessionId GetSessionId() { return id; };
    std::string GetNickname() { return nickname; };
};

#endif // EXTICHU_PLAYER_H_