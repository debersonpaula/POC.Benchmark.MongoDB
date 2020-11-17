db.createUser({
  user: "user",
  pwd: "user",
  roles: [
    {
      role: "readWrite",
      db: "DB_BY_COLLECTIONS",
    },
  ],
});
