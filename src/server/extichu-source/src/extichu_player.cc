#include "extichu_player.h"

namespace extichu
{
    namespace {
        static boost::atomic<int> player_id;
    }

    ExtPlayer::ExtPlayer(SessionId _id, std::string _nickname) 
    : sessionId(_id), nickname(_nickname) 
    {
        id = player_id.fetch_add(1);
    }
}