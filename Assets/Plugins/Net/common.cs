//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: common.proto
namespace WNet
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"msg_troop_attribute")]
  public partial class msg_troop_attribute : global::ProtoBuf.IExtensible
  {
    public msg_troop_attribute() {}
    
    private uint _army_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"army_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint army_id
    {
      get { return _army_id; }
      set { _army_id = value; }
    }
    private uint _exp;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"exp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint exp
    {
      get { return _exp; }
      set { _exp = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"resource")]
  public partial class resource : global::ProtoBuf.IExtensible
  {
    public resource() {}
    
    private uint _type = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint type
    {
      get { return _type; }
      set { _type = value; }
    }
    private ulong _num = default(ulong);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    public ulong num
    {
      get { return _num; }
      set { _num = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"mine")]
  public partial class mine : global::ProtoBuf.IExtensible
  {
    public mine() {}
    
    private uint _id = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _last_take_time = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"last_take_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint last_take_time
    {
      get { return _last_take_time; }
      set { _last_take_time = value; }
    }
    private uint _resource_level = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"resource_level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint resource_level
    {
      get { return _resource_level; }
      set { _resource_level = value; }
    }
    private uint _resource_type = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"resource_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint resource_type
    {
      get { return _resource_type; }
      set { _resource_type = value; }
    }
    private uint _resource_remain = default(uint);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"resource_remain", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint resource_remain
    {
      get { return _resource_remain; }
      set { _resource_remain = value; }
    }
    private WNet.int_pos _pos = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"pos", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public WNet.int_pos pos
    {
      get { return _pos; }
      set { _pos = value; }
    }
    private WNet.resource _stock_resource = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"stock_resource", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public WNet.resource stock_resource
    {
      get { return _stock_resource; }
      set { _stock_resource = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"base_data")]
  public partial class base_data : global::ProtoBuf.IExtensible
  {
    public base_data() {}
    
    private WNet.base_state _base_state = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"base_state", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public WNet.base_state base_state
    {
      get { return _base_state; }
      set { _base_state = value; }
    }
    private ulong _last_ts = default(ulong);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"last_ts", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    public ulong last_ts
    {
      get { return _last_ts; }
      set { _last_ts = value; }
    }
    private uint _base_id = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"base_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint base_id
    {
      get { return _base_id; }
      set { _base_id = value; }
    }
    private WNet.int_pos _base_coord = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"base_coord", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public WNet.int_pos base_coord
    {
      get { return _base_coord; }
      set { _base_coord = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"base_state")]
  public partial class base_state : global::ProtoBuf.IExtensible
  {
    public base_state() {}
    
    private bool _is_destory = (bool)false;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"is_destory", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue((bool)false)]
    public bool is_destory
    {
      get { return _is_destory; }
      set { _is_destory = value; }
    }
    private string _attacker_name = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"attacker_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string attacker_name
    {
      get { return _attacker_name; }
      set { _attacker_name = value; }
    }
    private ulong _destroy_time = default(ulong);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"destroy_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    public ulong destroy_time
    {
      get { return _destroy_time; }
      set { _destroy_time = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"bag_data")]
  public partial class bag_data : global::ProtoBuf.IExtensible
  {
    public bag_data() {}
    
    private readonly global::System.Collections.Generic.List<WNet.resource> _resource = new global::System.Collections.Generic.List<WNet.resource>();
    [global::ProtoBuf.ProtoMember(1, Name=@"resource", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.resource> resource
    {
      get { return _resource; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"dungeon_data")]
  public partial class dungeon_data : global::ProtoBuf.IExtensible
  {
    public dungeon_data() {}
    
    private WNet.guard_i _cur_guard = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"cur_guard", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public WNet.guard_i cur_guard
    {
      get { return _cur_guard; }
      set { _cur_guard = value; }
    }
    private readonly global::System.Collections.Generic.List<WNet.dungeon_i> _dungeon_list = new global::System.Collections.Generic.List<WNet.dungeon_i>();
    [global::ProtoBuf.ProtoMember(2, Name=@"dungeon_list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.dungeon_i> dungeon_list
    {
      get { return _dungeon_list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"guard_i")]
  public partial class guard_i : global::ProtoBuf.IExtensible
  {
    public guard_i() {}
    
    private uint _dungeon_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"dungeon_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint dungeon_id
    {
      get { return _dungeon_id; }
      set { _dungeon_id = value; }
    }
    private uint _guard_id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"guard_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint guard_id
    {
      get { return _guard_id; }
      set { _guard_id = value; }
    }
    private WNet.dungeon_state _state = WNet.dungeon_state.dungeon_locked;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"state", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(WNet.dungeon_state.dungeon_locked)]
    public WNet.dungeon_state state
    {
      get { return _state; }
      set { _state = value; }
    }
    private ulong _uniq_id = default(ulong);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"uniq_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    public ulong uniq_id
    {
      get { return _uniq_id; }
      set { _uniq_id = value; }
    }
    private WNet.guard_process_record _process_record = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"process_record", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public WNet.guard_process_record process_record
    {
      get { return _process_record; }
      set { _process_record = value; }
    }
    private WNet.guard_end_info _guard_end_info = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"guard_end_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public WNet.guard_end_info guard_end_info
    {
      get { return _guard_end_info; }
      set { _guard_end_info = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"guard_process_record")]
  public partial class guard_process_record : global::ProtoBuf.IExtensible
  {
    public guard_process_record() {}
    
    private readonly global::System.Collections.Generic.List<WNet.resource> _battle_rewards = new global::System.Collections.Generic.List<WNet.resource>();
    [global::ProtoBuf.ProtoMember(1, Name=@"battle_rewards", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.resource> battle_rewards
    {
      get { return _battle_rewards; }
    }
  
    private uint _kill_army_num = (uint)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"kill_army_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((uint)0)]
    public uint kill_army_num
    {
      get { return _kill_army_num; }
      set { _kill_army_num = value; }
    }
    private readonly global::System.Collections.Generic.List<WNet.uint32_kv> _kill_army_list = new global::System.Collections.Generic.List<WNet.uint32_kv>();
    [global::ProtoBuf.ProtoMember(3, Name=@"kill_army_list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.uint32_kv> kill_army_list
    {
      get { return _kill_army_list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"dungeon_i")]
  public partial class dungeon_i : global::ProtoBuf.IExtensible
  {
    public dungeon_i() {}
    
    private uint _dungeon_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"dungeon_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint dungeon_id
    {
      get { return _dungeon_id; }
      set { _dungeon_id = value; }
    }
    private readonly global::System.Collections.Generic.List<WNet.guard_st> _available_guards = new global::System.Collections.Generic.List<WNet.guard_st>();
    [global::ProtoBuf.ProtoMember(2, Name=@"available_guards", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.guard_st> available_guards
    {
      get { return _available_guards; }
    }
  
    private WNet.dungeon_state _state = WNet.dungeon_state.dungeon_locked;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"state", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(WNet.dungeon_state.dungeon_locked)]
    public WNet.dungeon_state state
    {
      get { return _state; }
      set { _state = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"guard_st")]
  public partial class guard_st : global::ProtoBuf.IExtensible
  {
    public guard_st() {}
    
    private uint _guard_id = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"guard_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint guard_id
    {
      get { return _guard_id; }
      set { _guard_id = value; }
    }
    private WNet.dungeon_state _state = WNet.dungeon_state.dungeon_locked;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"state", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(WNet.dungeon_state.dungeon_locked)]
    public WNet.dungeon_state state
    {
      get { return _state; }
      set { _state = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"map_object_index")]
  public partial class map_object_index : global::ProtoBuf.IExtensible
  {
    public map_object_index() {}
    
    private uint _owner_id = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"owner_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint owner_id
    {
      get { return _owner_id; }
      set { _owner_id = value; }
    }
    private uint _obj_id = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"obj_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint obj_id
    {
      get { return _obj_id; }
      set { _obj_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"msg_map_player_info")]
  public partial class msg_map_player_info : global::ProtoBuf.IExtensible
  {
    public msg_map_player_info() {}
    
    private uint _uid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private uint _camp;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"camp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint camp
    {
      get { return _camp; }
      set { _camp = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"guard_end_info")]
  public partial class guard_end_info : global::ProtoBuf.IExtensible
  {
    public guard_end_info() {}
    
    private WNet.guard_rslt _result = WNet.guard_rslt.guard_rslt_fail;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(WNet.guard_rslt.guard_rslt_fail)]
    public WNet.guard_rslt result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<WNet.resource> _first_rewards = new global::System.Collections.Generic.List<WNet.resource>();
    [global::ProtoBuf.ProtoMember(2, Name=@"first_rewards", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.resource> first_rewards
    {
      get { return _first_rewards; }
    }
  
    private readonly global::System.Collections.Generic.List<WNet.resource> _battle_rewards = new global::System.Collections.Generic.List<WNet.resource>();
    [global::ProtoBuf.ProtoMember(3, Name=@"battle_rewards", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.resource> battle_rewards
    {
      get { return _battle_rewards; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"produce_data")]
  public partial class produce_data : global::ProtoBuf.IExtensible
  {
    public produce_data() {}
    
    private readonly global::System.Collections.Generic.List<WNet.produce_unit> _produce_queue = new global::System.Collections.Generic.List<WNet.produce_unit>();
    [global::ProtoBuf.ProtoMember(1, Name=@"produce_queue", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.produce_unit> produce_queue
    {
      get { return _produce_queue; }
    }
  
    private uint _start_time;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"start_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint start_time
    {
      get { return _start_time; }
      set { _start_time = value; }
    }
    private readonly global::System.Collections.Generic.List<WNet.uint32_kvl> _army_template = new global::System.Collections.Generic.List<WNet.uint32_kvl>();
    [global::ProtoBuf.ProtoMember(3, Name=@"army_template", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<WNet.uint32_kvl> army_template
    {
      get { return _army_template; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"produce_unit")]
  public partial class produce_unit : global::ProtoBuf.IExtensible
  {
    public produce_unit() {}
    
    private WNet.uint32_kv _key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"key", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public WNet.uint32_kv key
    {
      get { return _key; }
      set { _key = value; }
    }
    private uint _left_time;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"left_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint left_time
    {
      get { return _left_time; }
      set { _left_time = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"tech_effect")]
  public partial class tech_effect : global::ProtoBuf.IExtensible
  {
    public tech_effect() {}
    
    private uint _effect_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"effect_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint effect_id
    {
      get { return _effect_id; }
      set { _effect_id = value; }
    }
    private uint _effect_level;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"effect_level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint effect_level
    {
      get { return _effect_level; }
      set { _effect_level = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"uint32_kv")]
  public partial class uint32_kv : global::ProtoBuf.IExtensible
  {
    public uint32_kv() {}
    
    private uint _key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"key", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint key
    {
      get { return _key; }
      set { _key = value; }
    }
    private uint _value;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"value", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint value
    {
      get { return _value; }
      set { _value = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"uint32_kvl")]
  public partial class uint32_kvl : global::ProtoBuf.IExtensible
  {
    public uint32_kvl() {}
    
    private uint _key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"key", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint key
    {
      get { return _key; }
      set { _key = value; }
    }
    private readonly global::System.Collections.Generic.List<uint> _list = new global::System.Collections.Generic.List<uint>();
    [global::ProtoBuf.ProtoMember(2, Name=@"list", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<uint> list
    {
      get { return _list; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"uint64_kv")]
  public partial class uint64_kv : global::ProtoBuf.IExtensible
  {
    public uint64_kv() {}
    
    private ulong _key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"key", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong key
    {
      get { return _key; }
      set { _key = value; }
    }
    private ulong _value;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"value", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong value
    {
      get { return _value; }
      set { _value = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"int_pos")]
  public partial class int_pos : global::ProtoBuf.IExtensible
  {
    public int_pos() {}
    
    private uint _x = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"x", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint x
    {
      get { return _x; }
      set { _x = value; }
    }
    private uint _y = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"y", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint y
    {
      get { return _y; }
      set { _y = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"plat_type")]
    public enum plat_type
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"self", Value=0)]
      self = 0
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"object_type")]
    public enum object_type
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"army", Value=1)]
      army = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"building", Value=2)]
      building = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"player_uint_data_type")]
    public enum player_uint_data_type
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"guide_status", Value=1)]
      guide_status = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"map_teleport_status", Value=2)]
      map_teleport_status = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"msg_down_op_code")]
    public enum msg_down_op_code
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"empty_to_c", Value=1)]
      empty_to_c = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"heartbeat_reply_to_c", Value=2)]
      heartbeat_reply_to_c = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"login_reply_to_c", Value=3)]
      login_reply_to_c = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"playerdata_reply_to_c", Value=4)]
      playerdata_reply_to_c = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"mapdata_reply_to_c", Value=5)]
      mapdata_reply_to_c = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"add_mapobject_reply_to_c", Value=6)]
      add_mapobject_reply_to_c = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"move_reply_to_c", Value=7)]
      move_reply_to_c = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"stopmove_reply_to_c", Value=8)]
      stopmove_reply_to_c = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"attack_reply_to_c", Value=9)]
      attack_reply_to_c = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"mapobject_dead_notify_to_c", Value=10)]
      mapobject_dead_notify_to_c = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"morale_recover_reply_to_c", Value=11)]
      morale_recover_reply_to_c = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"map_player_add_notify_to_c", Value=12)]
      map_player_add_notify_to_c = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"lowmorale_state_reply_to_c", Value=13)]
      lowmorale_state_reply_to_c = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"resdata_reply_to_c", Value=14)]
      resdata_reply_to_c = 14,
            
      [global::ProtoBuf.ProtoEnum(Name=@"start_move_notify_to_c", Value=15)]
      start_move_notify_to_c = 15,
            
      [global::ProtoBuf.ProtoEnum(Name=@"collect_res_reply_to_c", Value=16)]
      collect_res_reply_to_c = 16,
            
      [global::ProtoBuf.ProtoEnum(Name=@"resource_update_to_c", Value=17)]
      resource_update_to_c = 17,
            
      [global::ProtoBuf.ProtoEnum(Name=@"explored_area_reply_to_c", Value=18)]
      explored_area_reply_to_c = 18,
            
      [global::ProtoBuf.ProtoEnum(Name=@"error_reply_to_c", Value=19)]
      error_reply_to_c = 19,
            
      [global::ProtoBuf.ProtoEnum(Name=@"kill_monster_res_update_to_c", Value=20)]
      kill_monster_res_update_to_c = 20,
            
      [global::ProtoBuf.ProtoEnum(Name=@"tech_reply_to_c", Value=21)]
      tech_reply_to_c = 21,
            
      [global::ProtoBuf.ProtoEnum(Name=@"recruit_reply_to_c", Value=25)]
      recruit_reply_to_c = 25,
            
      [global::ProtoBuf.ProtoEnum(Name=@"update_map_object_notify_to_c", Value=28)]
      update_map_object_notify_to_c = 28,
            
      [global::ProtoBuf.ProtoEnum(Name=@"revive_reply_to_c", Value=29)]
      revive_reply_to_c = 29,
            
      [global::ProtoBuf.ProtoEnum(Name=@"map_object_pos_notify_to_c", Value=30)]
      map_object_pos_notify_to_c = 30,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ranking_reply_to_c", Value=31)]
      ranking_reply_to_c = 31,
            
      [global::ProtoBuf.ProtoEnum(Name=@"task_reply_to_c", Value=32)]
      task_reply_to_c = 32,
            
      [global::ProtoBuf.ProtoEnum(Name=@"task_get_award_reply_to_c", Value=33)]
      task_get_award_reply_to_c = 33,
            
      [global::ProtoBuf.ProtoEnum(Name=@"task_update_progress_notify_to_c", Value=34)]
      task_update_progress_notify_to_c = 34,
            
      [global::ProtoBuf.ProtoEnum(Name=@"army_combine_reply_to_c", Value=35)]
      army_combine_reply_to_c = 35,
            
      [global::ProtoBuf.ProtoEnum(Name=@"change_army_id_reply_to_c", Value=36)]
      change_army_id_reply_to_c = 36,
            
      [global::ProtoBuf.ProtoEnum(Name=@"building_create_reply_to_c", Value=37)]
      building_create_reply_to_c = 37,
            
      [global::ProtoBuf.ProtoEnum(Name=@"building_init_reply_to_c", Value=38)]
      building_init_reply_to_c = 38,
            
      [global::ProtoBuf.ProtoEnum(Name=@"recycle_army_reply_to_c", Value=39)]
      recycle_army_reply_to_c = 39,
            
      [global::ProtoBuf.ProtoEnum(Name=@"recycle_building_reply_to_c", Value=40)]
      recycle_building_reply_to_c = 40,
            
      [global::ProtoBuf.ProtoEnum(Name=@"movebase_reply_to_c", Value=41)]
      movebase_reply_to_c = 41,
            
      [global::ProtoBuf.ProtoEnum(Name=@"base_state_reply_to_c", Value=42)]
      base_state_reply_to_c = 42,
            
      [global::ProtoBuf.ProtoEnum(Name=@"change_map_reply_to_c", Value=43)]
      change_map_reply_to_c = 43,
            
      [global::ProtoBuf.ProtoEnum(Name=@"change_monster_battle_state_to_c", Value=44)]
      change_monster_battle_state_to_c = 44,
            
      [global::ProtoBuf.ProtoEnum(Name=@"update_rank_score_to_c", Value=45)]
      update_rank_score_to_c = 45,
            
      [global::ProtoBuf.ProtoEnum(Name=@"update_exp_to_c", Value=46)]
      update_exp_to_c = 46,
            
      [global::ProtoBuf.ProtoEnum(Name=@"change_unique_id_to_c", Value=47)]
      change_unique_id_to_c = 47,
            
      [global::ProtoBuf.ProtoEnum(Name=@"map_object_teleport_to_c", Value=48)]
      map_object_teleport_to_c = 48,
            
      [global::ProtoBuf.ProtoEnum(Name=@"error_info_reply_to_c", Value=49)]
      error_info_reply_to_c = 49,
            
      [global::ProtoBuf.ProtoEnum(Name=@"enter_guard_reply_to_c", Value=51)]
      enter_guard_reply_to_c = 51,
            
      [global::ProtoBuf.ProtoEnum(Name=@"exit_guard_reply_to_c", Value=52)]
      exit_guard_reply_to_c = 52,
            
      [global::ProtoBuf.ProtoEnum(Name=@"get_guard_mapdata_reply_to_c", Value=53)]
      get_guard_mapdata_reply_to_c = 53,
            
      [global::ProtoBuf.ProtoEnum(Name=@"end_guard_notify_to_c", Value=54)]
      end_guard_notify_to_c = 54,
            
      [global::ProtoBuf.ProtoEnum(Name=@"get_battle_reward_reply_c", Value=55)]
      get_battle_reward_reply_c = 55,
            
      [global::ProtoBuf.ProtoEnum(Name=@"enter_around_notify_to_c", Value=60)]
      enter_around_notify_to_c = 60,
            
      [global::ProtoBuf.ProtoEnum(Name=@"exit_around_notify_to_c", Value=61)]
      exit_around_notify_to_c = 61,
            
      [global::ProtoBuf.ProtoEnum(Name=@"mine_reply_to_c", Value=65)]
      mine_reply_to_c = 65
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"dungeon_state")]
    public enum dungeon_state
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"dungeon_locked", Value=0)]
      dungeon_locked = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"dungeon_unlocked", Value=1)]
      dungeon_unlocked = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"dungeon_active", Value=2)]
      dungeon_active = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"dungeon_complete", Value=3)]
      dungeon_complete = 3
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"guard_rslt")]
    public enum guard_rslt
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"guard_rslt_pass", Value=1)]
      guard_rslt_pass = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"guard_rslt_fail", Value=2)]
      guard_rslt_fail = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"recruit_type")]
    public enum recruit_type
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"recruit_nornal", Value=1)]
      recruit_nornal = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"recruit_auto", Value=2)]
      recruit_auto = 2
    }
  
}