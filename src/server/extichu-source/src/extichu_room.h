#ifndef EXTICHU_ROOM_H_
#define EXTICHU_ROOM_H_

#include <map>
#include <vector>
#include <queue>
#include <functional>

#include <boost/atomic.hpp>
#include <boost/function.hpp>

namespace extichu
{

#define MAX_PLAYER 4
class ExtPlayer;

class ExtRoom
{
public:
    ExtRoom();
    
private:
    int id;
    std::vector<Ptr<ExtPlayer> > players;

public:
    bool AddPlayer(Ptr<ExtPlayer> _player);
    bool IsFull() { return players.size() == MAX_PLAYER; }
    int  GetId() { return id ; }
    void EachPlayer(boost::function<void (Ptr<ExtPlayer> _player)> &&_func);

    // 대기중인 방에 들어간다. 없으면 방을 새로 만들고 다른 사람을 기다린다.
    static Ptr<ExtRoom> JoinOrCreateRoom(Ptr<ExtPlayer> _player);
};

}

#endif // EXTICHU_ROOM_H_