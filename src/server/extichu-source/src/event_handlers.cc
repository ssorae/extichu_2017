// PLEASE ADD YOUR EVENT HANDLER DECLARATIONS HERE.

#include "event_handlers.h"

#include <funapi.h>
#include <gflags/gflags.h>
#include <glog/logging.h>

#include "extichu_loggers.h"
#include "extichu_messages.pb.h"

#include "extichu_player.h"
#include "extichu_room.h"


// You can differentiate game server flavors.
DECLARE_string(app_flavor);


namespace extichu {

////////////////////////////////////////////////////////////////////////////////
// Session open/close handlers
////////////////////////////////////////////////////////////////////////////////

void OnSessionOpened(const Ptr<Session> &session) {
  logger::SessionOpened(to_string(session->id()), WallClock::Now());
}


void OnSessionClosed(const Ptr<Session> &session, SessionCloseReason reason) {
  logger::SessionClosed(to_string(session->id()), WallClock::Now());

  if (reason == kClosedForServerDid) {
    // Server has called session->Close().
  } else if (reason == kClosedForIdle) {
    // The session has been idle for long time.
  } else if (reason == kClosedForUnknownSessionId) {
    // The session was invalid.
  }
}



////////////////////////////////////////////////////////////////////////////////
// Client message handlers.
//
// (Just for your reference. Please replace with your own.)
////////////////////////////////////////////////////////////////////////////////

void OnJoinMatch(const Ptr<Session> &session, const Ptr<FunMessage> &message) 
{
  const CSJoinMatch &msg = message->GetExtension(cs_join_match);

  SessionId id = session->id();
  std::string nickname = msg.nickname();
  Ptr<ExtPlayer> player = boost::make_shared<ExtPlayer>(id, nickname);

  // 이 함수 안에서 이미 방 안에 있는 사람들에게 메시지 보냄.
  auto room = ExtRoom::JoinOrCreateRoom(player);
  int count = 0;

  Ptr<FunMessage> retMsg(new FunMessage);
  SCJoinMatch *join = retMsg->MutableExtension(sc_join_match);
  join->set_result(ErrorCode::EC_OK);
  PbRoomState* state = join->mutable_room_state();
  
  auto writeState = [&count, state](Ptr<ExtPlayer> _player){
    switch(count)
    {
      case 0:
      {
        state->set_nickname_1(_player->GetNickname());
      }
      break;
      case 1:
      {
        state->set_nickname_2(_player->GetNickname());
      }
      break;
      case 2:
      {
        state->set_nickname_3(_player->GetNickname());
      }
      break;
      case 3:
      {
        state->set_nickname_4(_player->GetNickname());
      }
      break;
    }
    ++count;
  };  
  room->EachPlayer(writeState);

  session->SendMessage(sc_join_match, retMsg);
}

////////////////////////////////////////////////////////////////////////////////
// Timer handler.
//
// (Just for your reference. Please replace with your own.)
////////////////////////////////////////////////////////////////////////////////

void OnTick(const Timer::Id &timer_id, const WallClock::Value &clock) {
  // PLACE HERE YOUR TICK HANDLER CODE.
}




////////////////////////////////////////////////////////////////////////////////
// Extend the function below with your handlers.
////////////////////////////////////////////////////////////////////////////////

void RegisterEventHandlers() {
  /*
   * Registers handlers for session close/open events.
   */
  {
    HandlerRegistry::Install2(OnSessionOpened, OnSessionClosed);
    HandlerRegistry::Register2(cs_join_match, OnJoinMatch);
  }


  /*
   * Registers handlers for messages from the client.
   *
   * Handlers below are just for you reference.
   * Feel free to delete them and replace with your own.
   */
  {
  }


  /*
   * Registers a timer.
   *
   * Below demonstrates a repeating timer. One-shot timer is also available.
   * Please see the Timer class.
   */
  {
    Timer::ExpireRepeatedly(boost::posix_time::seconds(1), extichu::OnTick);
  }
}

}  // namespace extichu
