#include "extichu_room.h"

namespace extichu
{
    Ptr<ExtRoom> JoinOrCreateRoom(Ptr<ExtPlayer> _player)
    {
        if(the_waiting_rooms.size() > 0) {
            // 대기중 방이 있으면 쪼인
            auto room = the_waiting_rooms.front();

            // 방에 플레이어 추가
            room->AddPlayer(_player);

            if(room->IsFull())
            {
                the_waiting_rooms.pop();
                the_playing_rooms.emplace(room->GetId(), room);
            }
        }
        else {
            // 대기중 방이 없으면 새 방 생성
            int id = room_id.fetch_add(1);
            auto room = std::make_shared<Room>(id);
            room->AddPlayer(_player);

            the_waiting_rooms.push(room);
        }
    }

    bool ExtRoom::AddPlayer(Ptr<ExtPlayer> _player)
    {   
        if(players.size() == MAX_PLAYER)
            return false;
        
        auto id = _player.GetSessionid();
        const std::string nick = _player.GetNickname();

        // 이미 방 안에 있는 사람들에게 조인 메시지 전송
        Ptr<FunMessage> message(new FunMessage);
        SCPlayerJoined *joined = message->MutableExtension(sc_player_joined);
        joined->set_player_id(id);
        joined->set_nickname(nick);

        auto sendJoined = [message](Ptr<ExtPlayer> _player){
            auto session = Session::Find(_player.GetId());
            session->SendMessage(sc_player_joined, message);
        }
        room.EachPlayer(sendJoined);

        players.emplace_back(_player);
        return true;
    }

    void ExtRoom::EachPlayer(boost::function<void (Ptr<ExtPlayer> _player)> _func)
    {
        for(auto it : players)
        {
            _func(it);
        }
    }
}