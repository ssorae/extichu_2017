#include <map>
#include <vector>
#include <queue>
#include <functional>

#include <boost/atomic.hpp>
#include <boost/function.hpp>

#include "extichu_player.h"

#ifndef EXTICHU_ROOM_H_
#define EXTICHU_ROOM_H_

namespace extichu
{

#define MAX_PLAYER 4

class ExtRoom
{
public:
    ExtRoom(int _id): id(_id) {
        players.resize(MAX_PLAYER);
    }
    
private:
    int id;
    std::vector<Ptr<ExtPlayer> > players;

public:
    bool AddPlayer(Ptr<ExtPlayer> _player);
    bool IsFull() { return players.size() == MAX_PLAYER; }
    int  GetId() { return id ; }
    void EachPlayer(boost::function<void (Ptr<ExtPlayer> _player)> _func);
};

static boost::atomic<int> room_id;
static std::queue<Ptr<ExtRoom> > the_waiting_rooms;
static std::map<int, Ptr<ExtRoom> > the_playing_rooms;

// 대기중인 방에 들어간다. 없으면 방을 새로 만들고 다른 사람을 기다린다.
Ptr<ExtRoom> JoinOrCreateRoom(Ptr<ExtPlayer> _player);
}

#endif // EXTICHU_ROOM_H_