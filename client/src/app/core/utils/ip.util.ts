export function intToIpString(ip: number): string {
  return [24, 16, 8, 0].map(n => (ip >> n) & 0xff).join(".");
}

export function ipStringToInt(ip: string): number {
  return ip.split('.').map(parseFloat).reduce((total, part) => total * 256 + part);
}

export function isIpStringValid(ip: string): boolean {
  return new RegExp('^(?!0)(?!.*\\.$)((1?\\d?\\d|25[0-5]|2[0-4]\\d)(\\.|$)){4}$').test(ip);
}
