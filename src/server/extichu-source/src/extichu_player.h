#include <funapi.h>

#include <string>
#include <boost/atomic.hpp>

#ifndef EXTICHU_PLAYER_H_
#define EXTICHU_PLAYER_H_

namespace extichu {

class ExtPlayer {
public:
    ExtPlayer(SessionId _id, std::string _nickname);

private:
    int id = 0;
    SessionId sessionId;
    std::string nickname;

public:
    int GetId() { return id; };
    SessionId GetSessionId() { return sessionId; };
    std::string GetNickname() { return nickname; };
};

}

#endif // EXTICHU_PLAYER_H_