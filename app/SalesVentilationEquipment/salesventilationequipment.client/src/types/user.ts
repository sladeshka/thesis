export interface IUser {
  id: number
  isActive: boolean,
  name: string
  JWTToken: number
  info: string
}

export type userData = IUser;
